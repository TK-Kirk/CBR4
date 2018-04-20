using System;

namespace SurveyRouter.Logic.Data.Models
{
    public class RouterUser
    {
        public string First { get; set; }
        public string Last { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Email { get; set; }
        public string UniqueId { get; set; }
        public string Gender { get; set; }
        public DateTime?  DOB { get; set; }

        public string IpAddress { get; set; }

        public Guid? PrecisionSampleUserID { get; set; }

        public int RouterContactId { get; set; }
    }
}

