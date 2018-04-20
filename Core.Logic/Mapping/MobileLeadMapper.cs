using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBR.Core.Entities.Models;
using CBR.Core.Entities.ResourceModels;

namespace CBR.Core.Logic.Mapping
{
    public class MobileLeadMapper
    {
        public static void Map(UserRegistrationRequest userRegistrationRequest, MobilelLead mobileLead)
        {
            mobileLead.Firstname = userRegistrationRequest.Firstname;
            mobileLead.Lastname = userRegistrationRequest.Lastname;
            mobileLead.Address = userRegistrationRequest.Address;
            mobileLead.City = userRegistrationRequest.City;
            mobileLead.State = userRegistrationRequest.State;
            mobileLead.Zip = userRegistrationRequest.Zip;
            mobileLead.Gender = userRegistrationRequest.Gender;
            mobileLead.Dob = userRegistrationRequest.DOB;
            mobileLead.EmailAddress = userRegistrationRequest.EmailAddress;
        }

        internal static UserRegistrationRequest Map(MobilelLead mobileLead)
        {
            var u = new UserRegistrationRequest();
            u.Firstname = mobileLead.Firstname;
            u.Lastname = mobileLead.Lastname;
            u.Address = mobileLead.Address;
            u.City = mobileLead.City;
            u.State = mobileLead.State;
            u.Zip = mobileLead.Zip;
            u.Gender = mobileLead.Gender;
            u.DOB = mobileLead.Dob;
            u.EmailAddress = mobileLead.EmailAddress;
            return u;
        }
    }
}
