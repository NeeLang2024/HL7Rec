using System;
using System.Collections.Generic;

namespace HL7ProcessorWinForms
{
    public class HL7Message
    {
        public string MessageControlID { get; set; }
        public string MessageType { get; set; }
        public DateTime MessageDateTime { get; set; }
        public PatientInfo Patient { get; set; } = new PatientInfo();
        public PV1Info Visit { get; set; } = new PV1Info();
        public EVNInfo Event { get; set; } = new EVNInfo();
        public List<Observation> Observations { get; set; } = new List<Observation>();
        public List<Order> Orders { get; set; } = new List<Order>();

        public HL7Message()
        {
            Patient = new PatientInfo();
            Visit = new PV1Info();
            Event = new EVNInfo();
            Observations = new List<Observation>();
            Orders = new List<Order>();
        }
    }

    public class PatientInfo
    {
        public string PatientID { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
    }

    public class PV1Info
    {
        public string PatientClass { get; set; } // PV1-2: Patient Class (e.g., "I" for Inpatient)
        public string AssignedLocation { get; set; } // PV1-3: Assigned Patient Location
        public string AdmissionType { get; set; } // PV1-4: Admission Type
        public string AttendingDoctor { get; set; } // PV1-7: Attending Doctor
    }

    public class EVNInfo
    {
        public string EventTypeCode { get; set; } // EVN-1: Event Type Code
        public DateTime RecordedDateTime { get; set; } // EVN-2: Recorded Date/Time
    }

    public class Observation
    {
        public int SetID { get; set; } // OBX-1: Set ID - OBX
        public string ValueType { get; set; } // OBX-2: Value Type
        public string ObservationID { get; set; } // OBX-3: Observation Identifier
        public string Value { get; set; } // OBX-5: Observation Value
        public string Units { get; set; } // OBX-6: Units
        public string ReferenceRange { get; set; } // OBX-7: Reference Range (optional)
        public string AbnormalFlags { get; set; } // OBX-8: Abnormal Flags (optional)
    }

    public class Order
    {
        public string OrderControl { get; set; } // ORC-1: Order Control
        public string PlacerOrderNumber { get; set; } // ORC-2: Placer Order Number
        public string FillerOrderNumber { get; set; } // ORC-3: Filler Order Number
    }
}