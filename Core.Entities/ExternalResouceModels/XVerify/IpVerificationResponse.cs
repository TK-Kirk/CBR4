using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBR.Core.Entities.ExternalResouceModels.XVerify
{
    public class IpVerificationResponse
    {
        public bool IsValid
        {
            get
            {
                if (ipdata != null)
                {
                    return ipdata.status == "valid";
                }
                return false;
            }
        }

        public Ipdata ipdata { get; set; }
    }

    public class Ipdata
    {
        public string value { get; set; }
        public string proxy { get; set; }
        public int error { get; set; }
        public string status { get; set; }
        public int response_code { get; set; }
        public string message { get; set; }
        public string region { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string duration { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
    }

}
