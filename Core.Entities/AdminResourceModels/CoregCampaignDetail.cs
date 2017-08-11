using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBR.Core.Entities.AdminResourceModels
{
    public class CoregCampaignDetail
    {
        public int CoregCampaignId { get; set; } // CoregCampaignId (Primary key)
        public string Name { get; set; } // Name (length: 255)
        public int CoregPartnerId { get; set; } // CoregPartnerId
        public bool Active { get; set; } // Active
        public string PartnerName { get; set; }
    }
}
