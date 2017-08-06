using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CBR.Core.Entities.Enums;
using CBR.Core.Entities.ExternalResouceModels;
using CBR.Core.Entities.ExternalResouceModels.EngageIQ;
using CBR.Core.Entities.ExternalResouceModels.ProvideMedia;
using CBR.Core.Entities.Models;
using CBR.DataAccess;
using Newtonsoft.Json;

namespace CBR.Core.Logic.Managers
{
    public class PostManagerCenterfieldMedia : PostManagerBase
    {
        const string baseurl = "https://tracking.centerfield.com/post/";



        public CoregPostResponse SubmitLead(CoregPostRequestBase request, string ipAddress, bool isTest)
        {
            var lead = _db.CbrLeads.FirstOrDefault(c => c.CbrLeadId == request.CbrLeadId);

            if (lead == null)
            {
                return new CoregPostResponse() { Success = false, Other = $"LeadId {request.CbrLeadId} not found." };
            }

            var xverifyManager = new XVerifyManager();
            var zipResponse = xverifyManager.VerifyZipAndIpAddress(ipAddress, request.Zip, request.email);
            if (zipResponse.IpIsIrReputable)
            {
                return new CoregPostResponse() { Success = false, ipIsIrReputable = true };
            }

            if (zipResponse.NoMatch || zipResponse.ZipCodeInvalid)
            {
                return new CoregPostResponse() { Success = false, zipIpVerificationFailed = true, InvalidZip = zipResponse.ZipCodeInvalid };
            }
            UpdateLeadCityStateIP(xverifyManager, lead, ipAddress);

            string leadtimestamp = DateTime.Now.ToString("M/d/yyyy"); //m/d/yyyy h:mm:ss
            string postData = null;

            var r = request;
            var l = lead;

            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("subid", request.SubIdTag);
            postParameters.Add("first_name", lead.Firstname);
            postParameters.Add("last_name", lead.Lastname);
            postParameters.Add("email_address", request.email);
            postParameters.Add("contact_phone", lead.Phone);
            postParameters.Add("ip_address", ipAddress);

            switch (request.CampaignCodeId)
            {
                case CoregCampaignType.Centerfield_Sprint:
                    //postData =
                    //    $"&offer_id={request.CampaignCode}&subid={r.SubIdTag}&email_address={l.EmailAddress}&first_name={l.Firstname}&last_name={l.Lastname}&contact_phone={l.Phone}&ip_address={ipAddress}&timestamp={leadtimestamp}";
                    postParameters.Add("offer_id", request.CampaignCode);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (isTest)
            {
                postParameters.Add("is_test", true);
                //    postData += "&is_test=true";
            }
            string response = PostForm(postParameters, baseurl);
            if (!string.IsNullOrWhiteSpace(response))
            {
                if (response.ToLower().Contains("success"))
                {
                    UpdateLeadAccepted(r, l);
                    return new CoregPostResponse() { Success = true };
                }
                WriteCoregError("Centerfield", postData, baseurl, response);

                return new CoregPostResponse() { Success = true };
            }

            WriteCoregError("Centerfield", postData, baseurl, "No response.");
            return new CoregPostResponse() { Success = false, Other = "No response." };
        }


        private string PostForm(Dictionary<string, object> postParameters, string url)
        {

            // Generate post objects
            //Dictionary<string, object> postParameters = new Dictionary<string, object>();
            //postParameters.Add("filename", "People.doc");
            //postParameters.Add("fileformat", "doc");

            // Create request and receive response
            string postURL = url;
            string userAgent = "Cashback Research";
            HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(postURL, userAgent, postParameters);

            // Process response
            StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
            string fullResponse = responseReader.ReadToEnd();
            webResponse.Close();

            return fullResponse;
        }

    }
}
