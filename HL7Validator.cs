using NLog;
using Microsoft.Extensions.Configuration;

namespace HL7ProcessorWinForms
{
    public static class HL7Validator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly IConfiguration Configuration = Program.Configuration;
        private static readonly Dictionary<string, int> MinFieldsPerSegment;
        private static readonly Dictionary<string, List<int>> RequiredFields;

        static HL7Validator()
        {
            MinFieldsPerSegment = Configuration.GetSection("ValidationRules:MinFieldsPerSegment").Get<Dictionary<string, int>>() ?? new Dictionary<string, int>();
            RequiredFields = Configuration.GetSection("ValidationRules:RequiredFields").Get<Dictionary<string, List<int>>>() ?? new Dictionary<string, List<int>>();
        }

        public static bool ValidateHL7Message(string hl7Message)
        {
            if (string.IsNullOrWhiteSpace(hl7Message))
            {
                Logger.Warn("HL7 message is empty");
                return false;
            }

            if (!hl7Message.StartsWith("MSH"))
            {
                Logger.Warn("HL7 message does not start with MSH segment");
                return false;
            }

            string[] segments = hl7Message.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length < 2)
            {
                Logger.Warn("HL7 message contains insufficient segments");
                return false;
            }

            foreach (string segment in segments)
            {
                string[] fields = segment.Split('|');
                if (fields.Length < 1) continue;

                string segmentType = fields[0];
                if (MinFieldsPerSegment.ContainsKey(segmentType))
                {
                    if (fields.Length < MinFieldsPerSegment[segmentType])
                    {
                        Logger.Warn($"Segment {segmentType} has insufficient fields: {fields.Length}");
                        return false;
                    }
                }

                if (RequiredFields.ContainsKey(segmentType))
                {
                    foreach (int index in RequiredFields[segmentType])
                    {
                        if (index >= fields.Length || string.IsNullOrWhiteSpace(fields[index]))
                        {
                            Logger.Warn($"Segment {segmentType} missing required field at index {index}");
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}