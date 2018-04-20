using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBR.Core.Entities.ResourceModels
{
    public class UserRegistrationRequest
    {
        public string EmailAddress { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Gender { get; set; }
        public DateTime? DOB { get; set; }
    }
}
