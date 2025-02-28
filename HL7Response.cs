using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace HL7ProcessorWinForms
{
    public static class HL7Response
    {
        public static string GenerateACK(string messageType, string messageControlID)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string msh = $"MSH|^~\\&|HL7Processor|Facility|Receiver|Facility|{timestamp}||ACK^{messageType.Substring(4)}|{Guid.NewGuid()}|P|2.5";
            string msa = $"MSA|AA|{messageControlID}";
            return $"{msh}\r{msa}\r";
        }
    }
}