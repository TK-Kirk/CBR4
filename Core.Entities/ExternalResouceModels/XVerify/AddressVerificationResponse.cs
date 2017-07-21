using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBR.Core.Entities.ExternalResouceModels.XVerify
{
    public class AddressVerificationResponse
    {
        public bool IsValid
        {
            get
            {
                if (address != null)
                {
                    return address.status == "valid";
                }
                return false;
            }
        }

        public Address address { get; set; }
    }

    public class Address
    {
        public int error { get; set; }
        public string status { get; set; }
        public int response_code { get; set; }
        public string message { get; set; }
        public string duration { get; set; }
        public string address1 { get; set; }
        public string street { get; set; }
        public string zip { get; set; }
        public string reason_for_invalid { get; set; }
        public string city { get; set; }
        public string state { get; set; }
    }

}
