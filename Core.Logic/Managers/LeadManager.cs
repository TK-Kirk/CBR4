using CBR.Core.Entities.ResourceModels;
using CBR.Core.Logic.Mapping;
using CBR.DataAccess;
using CBR.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBR.Core.Entities.Models;

namespace CBR.Core.Logic.Managers
{
    public class LeadManager
    {
        private GloshareContext _db;

        public LeadManager()
        {
            _db = new GloshareContext();
        }
        public Lead GetLead(string emailaddress, string offerId, string affiliateId)
        {

            var cbrlead = _db.CbrLeads.FirstOrDefault(c => c.EmailAddress == emailaddress && (c.AffiliateId == null || c.AffiliateId == affiliateId));

            var mapped = LeadMapper.Map(cbrlead);

            return mapped;
        }

        public Lead AddOrUpdateCoregLead(Lead coreglead)
        {
            var lead = _db.CbrLeads.FirstOrDefault(l => l.EmailAddress == coreglead.Email && l.OfferId == coreglead.OfferId && l.AffiliateId == coreglead.AffiliateId);
            if (lead == null)
            {
                lead = new CbrLead();
                _db.CbrLeads.Add(lead);
            }
            else
            {
                if (coreglead.OfferId != "57048")
                {
                    coreglead.IsDuplicate = true;
                }
            }

            if (string.IsNullOrEmpty(lead.CountryId))
            {
                lead.CountryId = GetCountry(lead.Ip);
            }
            LeadMapper.Map(coreglead, lead);

            _db.SaveChanges();

            coreglead.CBRLeadId = lead.CbrLeadId;

            return coreglead;
        }

        public string GetCountry(string ip)
        {
            string[] arrIP = ip.Split('.');

            double ip0 = Convert.ToDouble(arrIP[0]);
            double ip1 = Convert.ToDouble(arrIP[1]);
            double ip2 = Convert.ToDouble(arrIP[2]);
            double ip3 = Convert.ToInt32(arrIP[3]);
            double factor0 = 16777216;
            double factor1 = 65536;
            double factor2 = 256;
            double iIPNum = (ip0 * factor0) + (ip1 * factor1) + (ip2 * factor2) + ip3;

            //iIPNum = arrIP(0) * 16777216 + arrIP(1) * 65536 + arrIP(2) * 256 + arrIP(3)
            var ipCountry = _db.IpCountries.FirstOrDefault(x => x.StartNum < iIPNum && x.EndNum > iIPNum);

            if(ipCountry != null && !string.IsNullOrWhiteSpace(ipCountry.CountryId))
            {
                return ipCountry.CountryId;
            }

            var xverify = new XVerifyRepository();
            return xverify.GetCountryFromIp(ip);
        }

        
    }
}
