using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBR.Core.Entities.AdminResourceModels;
using CBR.Core.Entities.Models;

namespace Admin.Logic.Mapping
{
    public class CoregMapper
    {
        public static CoregCampaignDetail Map(CoregCampaign coregCampaign)
        {
            return new CoregCampaignDetail()
            {
                CoregPartnerId = coregCampaign.CoregPartnerId,
                Active = coregCampaign.Active,
                CoregCampaignId = coregCampaign.CoregCampaignId,
                Name = coregCampaign.Name,
                PartnerName = coregCampaign.CoregPartner.Name
            };
        }
    }
}
