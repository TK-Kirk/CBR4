using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBR.Core.Entities.Enums;

namespace CBR.Core.Entities.ExternalResouceModels
{
    public class CoregPostRequestBase
    {
        public string email { get; set; }

        public int CbrLeadId { get; set; }

        public string SubIdTag { get; set; }

        public string CampaignCode { get; set; }
        
        public string Zip { get; set; }

        public CoregCampaignType CampaignCodeId { get; set; }

    }
}
