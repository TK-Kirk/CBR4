using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBR.Core.Entities.ExternalResouceModels.XVerify;
using CBR.Core.Entities.Models;
using CBR.Core.Entities.ResourceModels;
using CBR.Core.Logic.Mapping;
using CBR.DataAccess;

namespace CBR.Core.Logic.Managers
{
    public class MobileUserManager
    {
        public MobileSignupResponse SignUpUser(string email, string ipAddress)
        {
            var db = new GloshareContext();
            if (string.IsNullOrWhiteSpace(email))
            {
                return new MobileSignupResponse() { Message = "Email is blank." };
            }


            try
            {
                var xv = new XVerifyManager();
                if (xv.GetEmailVerification(email))
                {
                    string countryId = GetCountryId(ipAddress);
                    var response = new MobileSignupResponse();
                    response.ValidEmail = true;
                    MobilelLead existingLead = db.MobilelLeads.FirstOrDefault(m => m.EmailAddress.ToLower() == email);
                    if (existingLead == null)
                    {
                        email = email.Trim().ToLower();
                        existingLead = new MobilelLead() { EmailAddress = email, Ip = ipAddress, CountryId = countryId};
                        db.MobilelLeads.Add(existingLead);
                        db.SaveChanges();
                    }
                    return new MobileSignupResponse()
                    {
                        UserId = existingLead.UserId,
                        IsRegistered = existingLead.IsRegistered,
                        ValidEmail = true,
                        Message = "Success",
                        RegisterUrl = $"{ConfigurationManager.AppSettings["BaseWebUrl"]}/register?ug={existingLead.UserId}",
                        DailySurveysUrl = $"{ConfigurationManager.AppSettings["BaseWebUrl"]}/mysurveys?mu={existingLead.UserId}"
                    };
                }

                return new MobileSignupResponse() { ValidEmail = false, Message = "Email Invalid." };
            }
            catch (Exception e)
            {

                return new MobileSignupResponse() { Message = $"Error: {e.Message}" };
            }
        }

        private string GetCountryId(string ipAddress)
        {
            XVerifyManager xm = new XVerifyManager();
            string countryId = "US";
            if (!string.IsNullOrWhiteSpace(ipAddress))
            {
                countryId = xm.GetCountryFromIP(ipAddress);
                if (string.IsNullOrWhiteSpace(countryId))
                {
                    countryId = "US";
                }
            }

            return countryId;
        }

        public void Register(int mobileLeadId, UserRegistrationRequest userRegistrationRequest)
        {
            var db = new GloshareContext();
            var mobileLead = db.MobilelLeads.Find(mobileLeadId);

            MobileLeadMapper.Map(userRegistrationRequest, mobileLead);

            db.SaveChanges();

        }

        public int? Register(Guid userId, UserRegistrationRequest userRegistrationRequest)
        {
            var db = new GloshareContext();
            var mobileLead = db.MobilelLeads.FirstOrDefault(m=>m.UserId==userId);

            MobileLeadMapper.Map(userRegistrationRequest, mobileLead);
            
            db.SaveChanges();

            return mobileLead.RouterContactId;

        }

        public UserRegistrationRequest GetUser(Guid userId)
        {
            var db = new GloshareContext();

            var mobileLead =  db.MobilelLeads.FirstOrDefault(m => m.UserId == userId);

            return MobileLeadMapper.Map(mobileLead);
        }

        public Guid GetRouterContactIdFromMobileUser(Guid userId)
        {
            var db = new GloshareContext();
            var user = db.MobilelLeads.Where(m => m.UserId == userId).Select(r=>r.RouterContact).FirstOrDefault();
            return user.UniqueId;
        }


        public void SetRouterContact(int? routerContactId, Guid userId)
        {
            var db = new GloshareContext();
            var mobileLead = db.MobilelLeads.FirstOrDefault(m => m.UserId == userId);
            mobileLead.RouterContactId = routerContactId;
            db.SaveChanges();
        }
    }


}
