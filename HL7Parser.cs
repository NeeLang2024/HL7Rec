using NLog;
using System;
using System.Collections.Generic;

namespace HL7ProcessorWinForms
{
    public static class HL7Parser
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly Dictionary<string, Action<string[], HL7Message>> SegmentHandlers = new Dictionary<string, Action<string[], HL7Message>>
        {
            { "MSH", ParseMSH },
            { "PID", ParsePID },
            { "PV1", ParsePV1 },
            { "EVN", ParseEVN },
            { "OBX", ParseOBX },
            { "ORC", ParseORC }
        };

        public static HL7Message ParseHL7Message(string hl7Message)
        {
            var message = new HL7Message();
            string[] segments = hl7Message.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string segment in segments)
            {
                string[] fields = segment.Split('|');
                if (fields.Length < 1 || !SegmentHandlers.ContainsKey(fields[0]))
                {
                    Logger.Warn($"Unsupported or invalid segment: {fields[0]}");
                    continue;
                }

                try
                {
                    SegmentHandlers[fields[0]](fields, message);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, $"Error parsing segment {fields[0]}");
                }
            }

            Logger.Info($"Parsed message: ID={message.MessageControlID}, Type={message.MessageType}, DateTime={message.MessageDateTime}");
            return message;
        }

        private static void ParseMSH(string[] fields, HL7Message message)
        {
            if (fields.Length < 10) throw new ArgumentException("MSH segment has insufficient fields");
            message.MessageDateTime = DateTime.ParseExact(fields[6], "yyyyMMddHHmmss", null); // 日期时间在fields[6]
            message.MessageType = fields[8]; // 消息类型在fields[8]
            message.MessageControlID = fields[9]; // 消息ID在fields[9]
        }

        private static void ParsePID(string[] fields, HL7Message message)
        {
            if (fields.Length < 9) throw new ArgumentException("PID segment has insufficient fields");
            message.Patient.PatientID = fields[3];
            message.Patient.Name = fields[5];
            message.Patient.DateOfBirth = DateTime.ParseExact(fields[7], "yyyyMMdd", null);
            message.Patient.Gender = fields[8];
        }

        private static void ParsePV1(string[] fields, HL7Message message)
        {
            if (fields.Length < 8) throw new ArgumentException("PV1 segment has insufficient fields");
            message.Visit.PatientClass = fields.Length > 2 ? fields[2] : null; // PV1-2: Patient Class
            message.Visit.AssignedLocation = fields.Length > 3 ? fields[3] : null; // PV1-3: Assigned Patient Location
            message.Visit.AdmissionType = fields.Length > 4 ? fields[4] : null; // PV1-4: Admission Type
            message.Visit.AttendingDoctor = fields.Length > 7 ? fields[7] : null; // PV1-7: Attending Doctor
        }

        private static void ParseEVN(string[] fields, HL7Message message)
        {
            if (fields.Length < 3) throw new ArgumentException("EVN segment has insufficient fields");
            message.Event.EventTypeCode = fields[1]; // EVN-1: Event Type Code
            message.Event.RecordedDateTime = DateTime.ParseExact(fields[2], "yyyyMMddHHmmss", null); // EVN-2: Recorded Date/Time
        }

        private static void ParseOBX(string[] fields, HL7Message message)
        {
            if (fields.Length < 9) throw new ArgumentException("OBX segment has insufficient fields");
            message.Observations.Add(new Observation
            {
                SetID = int.Parse(fields[1]), // OBX-1: Set ID - OBX
                ValueType = fields[2], // OBX-2: Value Type
                ObservationID = fields[3], // OBX-3: Observation Identifier
                Value = fields[5], // OBX-5: Observation Value
                Units = fields.Length > 6 ? fields[6] : null, // OBX-6: Units
                ReferenceRange = fields.Length > 7 ? fields[7] : null, // OBX-7: Reference Range
                AbnormalFlags = fields.Length > 8 ? fields[8] : null // OBX-8: Abnormal Flags
            });
        }

        private static void ParseORC(string[] fields, HL7Message message)
        {
            if (fields.Length < 4) throw new ArgumentException("ORC segment has insufficient fields");
            message.Orders.Add(new Order
            {
                OrderControl = fields[1], // ORC-1: Order Control
                PlacerOrderNumber = fields[2], // ORC-2: Placer Order Number
                FillerOrderNumber = fields[3] // ORC-3: Filler Order Number
            });
        }
    }
}