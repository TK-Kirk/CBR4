using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.Logic.Mapping;
using CBR.Core.Entities.AdminResourceModels;
using CBR.Core.Entities.Models;
using CBR.DataAccess;

namespace Admin.Logic.Managers
{

    public class CoregManager
    {
        private GloshareContext _db;
        public CoregManager()
        {
            _db = new GloshareContext();
        }

        public IEnumerable<CoregCampaignDetail> GetCampaigns()
        {
            var campaigns = _db.CoregCampaigns.ToList();
            return campaigns.Select(c => CoregMapper.Map(c));
        }

        public bool SetCampaignActive(int id, bool active)
        {
            var campaign = _db.CoregCampaigns.First(c => c.CoregCampaignId == id);
            campaign.Active = active;
            _db.SaveChanges();
            return true;
        }
    }
}
