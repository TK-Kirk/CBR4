using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBR.Core.Entities.ExternalResouceModels.ProvideMedia;
using System.Web;
using System.Xml.Schema;
using CBR.DataAccess;
using CBR.DataAccess.Repositories;
using Newtonsoft.Json;

namespace CBR.Core.Logic.Managers
{
    public class PostManagerProvideMedia : PostManagerBase
    {
        const string BASE_URL = @"https://post.providestudents.com/incoming/leads?";
        const string CAMPAIGN_CODE_DEBT_COM = "wm9EdfezDE8RXU9Rxt21LA";
        const string CAMPAIGN_CODE_SPRING_POWER_GAS = "6lIEmSzGTZ-OW52pU7Ir5g";
        const string CAMPAIGN_CODE_DIRECT_ENERGY = "yGK2ea4AmDf1fVJbMg05kQ";
        GloshareContext _db = new GloshareContext();

        public ProvideMediaResponse SubmitProvideMediaLead(ProvideMediaRequest request, string ipAddress, bool isTest)
        {
            var lead = _db.CbrLeads.FirstOrDefault(l => l.CbrLeadId == request.CbrLeadId);

            if (lead == null)
            {
                return new ProvideMediaResponse(){Success = false, Other = $"LeadId {request.CbrLeadId} not found."};
            }

            XVerifyManager xvm = new XVerifyManager();
            var xverifyZipResult = xvm.VerifyZipCode(lead.EmailAddress, ipAddress, lead.Address, lead.Zip);
            if (!xverifyZipResult.IsValid)
            {
                if (xverifyZipResult.NoMatch)
                {
                    return new ProvideMediaResponse() { Success = false, Other = "Failed zip/ip verifiction." };
                }

                return new ProvideMediaResponse()
                {
                    Success = false,
                    InvalidAddress = xverifyZipResult.AddressInvalid,
                    InvalidZip = xverifyZipResult.ZipCodeInvalid,
                    Message = xverifyZipResult.Message
                };
            };

            //Update state/city/ip 
            if (string.IsNullOrWhiteSpace(lead.City) || string.IsNullOrWhiteSpace(lead.State))
            {
                //if the zip verification was not cached we can get it from that xverify call result
                //else we have to make new call to xverify
                if (xvm.AddressInfo != null)
                {
                    lead.City = xvm.AddressInfo.address.city;
                    lead.State = xvm.AddressInfo.address.state;
                }
                else
                {
                    var addressInfo  = xvm.GetAddressVerification(lead.Address, lead.Zip);
                    lead.City = addressInfo.address.city;
                    lead.State = addressInfo.address.state;
                    lead.Zip = addressInfo.address.zip;
                }
                _db.SaveChanges();
            }

            if (lead.Ip != ipAddress)
            {
                lead.Ip = ipAddress;
                _db.SaveChanges();
            }


            var phone = PreparePhoneForPost(lead.Phone);

            request.TrustedForm = HttpUtility.UrlEncode(request.TrustedForm);

            //EncodeContact(lead);

            string consent = "Yes";
            if (request.CampaignCode == CAMPAIGN_CODE_DEBT_COM)
            {
                consent = "1";
            }

            var gender = "";
            if (!string.IsNullOrWhiteSpace(lead.Gender))
            {
                gender = lead.Gender.Substring(0, 1).ToUpper();
            }
            var c = lead;


            string postData = $"campaign_code={request.CampaignCode}&lead[firstname]={c.Firstname}&lead[lastname]={c.Lastname}" +
                $"&lead[email]={c.EmailAddress}&lead[phone1]={phone}&lead[age]={GetAge(c.BirthDate)}" +
                $"&lead[gender]={gender}&lead_consent[tcpa_consent]={consent}&lead[ip]={c.Ip}&subid={request.SubIdTag}" +
                $"&lead_address[state]={c.State}&lead_address[address]={c.Address}&lead_address[city]={c.City}&lead_address[zip]={c.Zip}" +
                $"&lead[test]={isTest}&lead[media_type]=noncallcenter" +
                $"&lead_custom_value[trusted_form]={request.TrustedForm}";

            string response = Post(postData, BASE_URL);

            if (!string.IsNullOrWhiteSpace(response))
            {
                if (response.Contains("success"))
                {

                    return new ProvideMediaResponse() { Success = true };
                }
                string errorUrl = BASE_URL + postData;
                WriteCoregError("ProvideMedia", postData, errorUrl, response);

                return ParseResponse(response);
            }

            return new ProvideMediaResponse() { Success = false, Other = "No response." };

        }

        private ProvideMediaResponse ParseResponse(string response)
        {

            //PMResponse pmr = JsonConvert.DeserializeObject<PMResponse>(response);

            if (response.Contains("not valid for this offer"))
            {
                new ProvideMediaResponse() { Success = false, Other = "Not valid for offer." };
            }

            if (response.Contains("failure"))
            {
                if (response.Contains("lead_address[address]"))
                {
                    return new ProvideMediaResponse() { Success = false, InvalidAddress=true };
                }
                if (response.Contains("lead[phone1]"))
                {
                    return new ProvideMediaResponse() { Success = false, InvalidPhone = true };
                }
                if (response.Contains("lead_address[zip]"))
                {
                    return new ProvideMediaResponse() { Success = false, InvalidZip = true };
                }

            }

            return new ProvideMediaResponse() { Success = false, Other = "No respose." };


        }


        public void UpdateLeadWithProvideMediaData(ProvideMediaUpdateRequest request)
        {
            var lead = _db.CbrLeads.FirstOrDefault(l => l.CbrLeadId == request.RetryRequest.CbrLeadId);

            if (lead != null)
            {
                lead.Address = request.Address;
                lead.Zip = request.Zip;
                lead.Phone = request.Phone;
                _db.SaveChanges();
            }

        }
    }


    public class PMResponse
    {
        public string status { get; set; }
        public Reason reason { get; set; }
        public string reason_type { get; set; }
        public bool testing { get; set; }
    }

    public class Reason
    {
        public string[] lead_address { get; set; }
    }

}
