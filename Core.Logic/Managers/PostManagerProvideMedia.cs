using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBR.Core.Entities.ExternalResouceModels.ProvideMedia;
using System.Web;
using System.Xml.Schema;
using CBR.Core.Entities.ExternalResouceModels;
using CBR.Core.Entities.Models;
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

        public CoregPostResponse SubmitLead(ProvideMediaRequest request, string ipAddress, bool isTest)
        {
            var lead = _db.CbrLeads.FirstOrDefault(l => l.CbrLeadId == request.CbrLeadId);

            if (lead == null)
            {
                return new CoregPostResponse(){Success = false, Other = $"LeadId {request.CbrLeadId} not found."};
            }

            //XVerifyManager xvm = new XVerifyManager();
            //var xverifyZipResult = xvm.VerifyZipCode(lead.EmailAddress, ipAddress, lead.Address, lead.Zip);
            //if (!xverifyZipResult.IsValid)
            //{
            //    if (xverifyZipResult.NoMatch)
            //    {
            //        return new ProvideMediaResponse() { Success = false, Other = "Failed zip/ip verifiction." };
            //    }

            //    return new ProvideMediaResponse()
            //    {
            //        Success = false,
            //        InvalidAddress = xverifyZipResult.AddressInvalid,
            //        InvalidZip = xverifyZipResult.ZipCodeInvalid,
            //        Message = xverifyZipResult.Message
            //    };
            //};

            var xverifyManager = new XVerifyManager();
            var zipResponse = xverifyManager.VerifyZipAndIpAddress(ipAddress, lead.Zip, request.email);
            if (zipResponse.IpIsIrReputable)
            {
                return new CoregPostResponse() { Success = false, ipIsIrReputable = true };
            }

            if (zipResponse.NoMatch || zipResponse.ZipCodeInvalid)
            {
                return new CoregPostResponse() { Success = false, zipIpVerificationFailed = true, InvalidZip = zipResponse.ZipCodeInvalid };
            }
            UpdateLeadCityStateIP(xverifyManager, lead, ipAddress);

            var phone = PreparePhoneForPost(lead.Phone);

            request.TrustedForm = HttpUtility.UrlEncode(request.TrustedForm);

            //EncodeContact(lead);

            string consent = "Yes";
            if (request.CampaignCode == CAMPAIGN_CODE_DEBT_COM)
            {
                consent = "1";
            }

            var gender = GenderGetSingleCharacter(lead);

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
                    UpdateLeadAccepted(request, c);
                    return new CoregPostResponse() { Success = true };
                }
                string errorUrl = BASE_URL + postData;
                WriteCoregError("ProvideMedia", postData, errorUrl, response);

                return ParseResponse(response);
            }

            return new CoregPostResponse() { Success = false, Other = "No response." };

        }




        private CoregPostResponse ParseResponse(string response)
        {

            //PMResponse pmr = JsonConvert.DeserializeObject<PMResponse>(response);

            if (response.Contains("not valid for this offer"))
            {
                //return true for success here because 
                //we don't want the data correction control to show 
                return new CoregPostResponse() { Success = true, Other = "Not valid for offer." };
            }

            if (response.Contains("failure"))
            {
                if (response.Contains("lead_address[address]"))
                {
                    return new CoregPostResponse() { Success = false, InvalidAddress=true };
                }
                if (response.Contains("lead[phone1]"))
                {
                    return new CoregPostResponse() { Success = false, InvalidPhone = true };
                }
                if (response.Contains("lead_address[zip]"))
                {
                    return new CoregPostResponse() { Success = false, InvalidZip = true };
                }

            }

            return new CoregPostResponse() { Success = false, Other = "No respose." };


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
