using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBR.Core.Entities.ExternalResouceModels.XVerify
{

    public class ZipcodeLookupResponse
    {
        public bool IsValid
        {
            get
            {
                if (zipdata != null)
                {
                    return zipdata.status == "valid";
                }
                return false;
            }
        }
        public Zipdata zipdata { get; set; }
    }

    public class Zipdata
    {
        public string value { get; set; }
        public string status { get; set; }
        public int error { get; set; }
        public int responsecode { get; set; }
        public string state { get; set; }
        public string county { get; set; }
        public string primary_city { get; set; }
        public string other_city { get; set; }
    }
}
