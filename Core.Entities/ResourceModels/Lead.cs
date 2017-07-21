using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBR.Core.Entities.ResourceModels
{
    public class Lead
    {
        public string Email { get; set; } // EmailAddress (Primary key) (length: 255)
        public string Firstname { get; set; } // Firstname (length: 100)
        public string Lastname { get; set; } // Lastname (length: 100)
        public string Address { get; set; } // Address (length: 100)
        public string Address2 { get; set; } // Address2 (length: 100)
        public string City { get; set; } // City (length: 100)
        public string State { get; set; } // State (length: 100)
        public string Zip { get; set; } // Zip (length: 100)
        public string Phone { get; set; } // Phone (length: 100)

        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; } // Gender (length: 1)
        public string Ethnicity { get; set; } // Ethnicity (length: 50)
        public string OfferId { get; set; } // OfferID (length: 100)
        public string AffiliateId { get; set; } // AffiliateID (length: 255)
        public string SubId { get; set; } // SubID (length: 255)
        public string CountryId { get; set; } // CountryID (Primary key) (length: 2)
        public string Ip { get; set; } // IP (length: 15)

        public string Device { get; set;}

    }
}
