using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBR.Core.Entities.ExternalResouceModels
{
    public class CoregPostResponse
    {
        public bool Success { get; set; }
        public bool InvalidPhone { get; set; }
        public bool InvalidZip { get; set; }
        public bool InvalidAddress { get; set; }

        public bool zipIpVerificationFailed { get; set; }
        public bool ipIsIrReputable { get; set; }
        public string Other { get; set; }

        public string Message { get; set; } //provide better info from xverify address verification
    }
}
