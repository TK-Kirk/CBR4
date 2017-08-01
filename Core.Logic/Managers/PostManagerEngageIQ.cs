using System;
using System.Collections.Generic;
using System.Linq;
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
    public class PostManagerEngageIQ : PostManagerBase
    {
        const string USERID = "Michaelh@webhenmedia.com";
        const string PASSWORD = "def@6zx44";
        const int Campaign_MySurvey = 2;
        const int Campaign_PaidForResearch = 393; //debt help
        const int Campaign_GlobalTest = 4;
        const int Campaign_PainGel = 401;
        const int Campaign_Toluna = 1;
        const string baseurl = "http://leadreactor.engageiq.com/sendLead/";
        const string basePostData = "?eiq_realtime=1&eiq_affiliate_id=295&rev_tracker=CD295";



        public CoregPostResponse SubmitLead(EngageIqRequest request, string ipAddress)
        {
            // use this to flag leads as not qualified 
            // due to wrong answers on custom questions
            bool postLead = true;
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
                return new CoregPostResponse() { Success = false, zipIpVerificationFailed = true, InvalidZip = zipResponse.ZipCodeInvalid};
            }
            UpdateLeadCityStateIP(xverifyManager, lead, ipAddress);


            string leadtimestamp = DateTime.Now.ToString("yyyyMMddHHmmss"); //20170719123212

            string postdate_YMD = DateTime.Now.ToString("yyyy-MM-dd"); //1971-01-03

            string gender = GenderGetSingleCharacter(lead);


            string dob_YMD_dash = null;
            string dob_YMD_slash = null;
            if (lead.BirthDate.HasValue)
            {
                dob_YMD_dash = lead.BirthDate.Value.ToString("yyyy-MM-dd"); //1971-01-03
                dob_YMD_slash = lead.BirthDate.Value.ToString("yyyy/MM/dd"); //1971/01/03
            }

            SwapSpacesForPlusSign(request, lead);

            string postData = null;
            //EngageIQ_Taxotere &s1=rev1&eiq_campaign_id=251&eiq_email=tk1@webhenmedia.com&first_name=Romnick&last_name=Sars&phone=4026804444&ip=192.168.1.1&q1=Yes&q2=No
            //Rail road cancer  &s1=rev1&eiq_campaign_id=632&eiq_email=Romnick21.Sar32@yahoo.com&first_name=Romnick&last_name=Sars&phone=3048806079&ip=192.168.1.1&q1=Yes
            //Hernia Mesh       &s1=rev1&eiq_campaign_id=548&eiq_email=Romnick21.Sar32@yahoo.com&first_name=Romnick&last_name=Sars&phone=3048806079&ip=192.168.1.1&state=AL&q1=Yes&q2=2004+or+later&q3=Yes&q4=0
            //Xarelto           &s1=rev1&eiq_campaign_id=514&eiq_email=Romnick21.Sar32@yahoo.com&first_name=Romnick&last_name=Sars&phone=2056532345&zip=35126&state=AL&q1=Yes&q2=No
            //back brace        &s1=rev1&eiq_campaign_id=341&eiq_email=tk66@webhenmedia.com     &first_name=Romnick&last_name=Sars&phone=4026804444&address=125+Broadway&zip=10016&city=New York&state=NY&ip=192.168.1.2
            //medical alert     &s1=rev1&eiq_campaign_id=561&eiq_email=Romnick21.Sar32@yahoo.com&first_name=Romnick&last_name=Sars&phone=3048806079&address=5911+Marches&zip=35126&city=Pinson  &state=AL&dob=1971-01-03&gender=M&datetime=2017-7-19&leadid=20170719123212
            //pain gel          &s1=rev1&eiq_campaign_id=401&eiq_email=Romnick21.Sar32@yahoo.com&first_name=Romnick&last_name=Sars&phone=2126532345&address=125+Broadway&zip=10016&city=New York&state=NY&ip=104.173.123.246&dob=06/11/1943&gender=M&
            //toluna            &s1=rev1&eiq_campaign_id=1&  eiq_email=Romnick21.Sar32@yahoo.com&first_name=Romnick&last_name=Sars&zip=01516&gender=M&birth_date=1960-05-16
            //motorveh accid    &s1=rev1&eiq_campaign_id=204&eiq_email=Romnick21.Sar32@yahoo.com&first_name=Romnick&last_name=Sars&phone=2056532345&address=5911+Marchester+circle&zip=35126&city=Pinson&state=AL&ip=104.173.123.246&lr=new&program_name=National+Injury+Bureau+MVA(1170)&program_id=1170&q1=Auto&q2=2015&q4=no&q5=Fred Panopio&q6=no&comments=I+was+hit+from+behind+by+a+truck&s1=yourSudID1&terms=Yes

            var r = request;
            var l = lead;

            switch (request.CampaignCodeId)
            {
                case CoregCampaignType.EngageIQ_Taxotere:
                    postLead = (r.Q1 == "Yes" && r.Q2 == "No");
                    postData =
                        $"&s1={r.SubIdTag}&eiq_campaign_id={r.CampaignCode}&eiq_email={l.EmailAddress}&first_name={l.Firstname}&last_name={l.Lastname}&phone={l.Phone}&ip={ipAddress}&q1={r.Q1}&q2={r.Q2}";
                    break;
                case CoregCampaignType.EngageIQ_RailroadCancer:
                    postData =
                    $"&s1={r.SubIdTag}&eiq_campaign_id={r.CampaignCode}&eiq_email={l.EmailAddress}&first_name={l.Firstname}&last_name={l.Lastname}&phone={l.Phone}&ip={ipAddress}&q1={r.Q1}";
                    break;
                case CoregCampaignType.EngageIQ_HernaMesh:
                    //only post lead if Q2 is 2004 or later
                    postLead = IsGreaterThan(r.Q2, 2003) && r.Q4 == "0";
                    postData =
                        $"&s1={r.SubIdTag}&eiq_campaign_id={r.CampaignCode}&eiq_email={l.EmailAddress}&first_name={l.Firstname}&last_name={l.Lastname}&phone={l.Phone}&state={l.State}&ip={ipAddress}&q1={r.Q1}&q2={r.Q2}";
                    break;
                case CoregCampaignType.EngageIQ_Xarelto:
                    postData =
                        $"&s1={r.SubIdTag}&eiq_campaign_id={r.CampaignCode}&eiq_email={l.EmailAddress}&first_name={l.Firstname}&last_name={l.Lastname}&phone={l.Phone}&zip={l.Zip}&state={l.State}&q1={r.Q1}&q2={r.Q2}";
                    break;
                case CoregCampaignType.EngageIQ_BackBrace:
                    postData =
                        $"&s1={r.SubIdTag}&eiq_campaign_id={r.CampaignCode}&eiq_email={l.EmailAddress}&first_name={l.Firstname}&last_name={l.Lastname}&phone={l.Phone}&zip={l.Zip}&address={l.Address}&city={l.City}&state={l.State}&ip={ipAddress}";
                    break;
                case CoregCampaignType.EngageIQ_MedicalAlert:
                    postData =
                        $"&s1={r.SubIdTag}&eiq_campaign_id={r.CampaignCode}&eiq_email={l.EmailAddress}&first_name={l.Firstname}&last_name={l.Lastname}&phone={l.Phone}&zip={l.Zip}&address={l.Address}&city={l.City}&state={l.State}&ip={ipAddress}&dob={dob_YMD_dash}&gender={gender}&datetime={postdate_YMD}&leadid={leadtimestamp}";
                    break;
                case CoregCampaignType.EngageIQ_PainGel:
                    postData =
                        $"&s1={r.SubIdTag}&eiq_campaign_id={r.CampaignCode}&eiq_email={l.EmailAddress}&first_name={l.Firstname}&last_name={l.Lastname}&phone={l.Phone}&zip={l.Zip}&address={l.Address}&city={l.City}&state={l.State}&ip={ipAddress}&dob={dob_YMD_slash}&gender={gender}";
                    break;
                case CoregCampaignType.EngageIQ_Toluna:
                    //needs pre ping
                    postData =
                        $"&s1={r.SubIdTag}&eiq_campaign_id={r.CampaignCode}&eiq_email={l.EmailAddress}&first_name={l.Firstname}&last_name={l.Lastname}&zip={l.Zip}&gender={gender}&birth_date={dob_YMD_dash}";
                    break;
                case CoregCampaignType.EngageIQ_MotorVehicleAccident:
                    postData =
                        $"&s1={r.SubIdTag}&eiq_campaign_id={r.CampaignCode}&eiq_email={l.EmailAddress}&first_name={l.Firstname}&last_name={l.Lastname}&phone={l.Phone}&zip={l.Zip}&address={l.Address}&city={l.City}&state={l.State}&ip={ipAddress}&lr=new&program_name=National+Injury+Bureau+MVA(1170)&program_id=1170&q1={r.Q1}&q2={r.Q2}&q3={r.Q3}&q4={r.Q4}&q5={r.Q5}&q6={r.Q6}&comments={r.Comments1}&terms=Yes";
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            postData = basePostData + postData;

            if (postLead)
            {
                string response = Get(baseurl + postData);
                if (!string.IsNullOrWhiteSpace(response))
                {
                    //dynamic dyn = JsonConvert.DeserializeObject(response);
                    if (response.ToLower().Contains("success"))
                    {
                        UpdateLeadAccepted(r, l);
                        return new CoregPostResponse() { Success = true };
                    }
                    string errorUrl = baseurl + postData;
                    WriteCoregError("EngageIQ", postData, errorUrl, response);

                    return new CoregPostResponse() { Success = true };
                }
                return new CoregPostResponse() { Success = false, Other = "No response." };
            }

            //lead was not posted due to wrong answers on custom questions
            return new CoregPostResponse() { Success = true };
        }




        private void SwapSpacesForPlusSign(EngageIqRequest request, CbrLead lead)
        {
            int sso = (int)StringSplitOptions.RemoveEmptyEntries;
            //Lead
            if (!string.IsNullOrWhiteSpace(lead.Address))
            {
                lead.Address = String.Join("+", lead.Address.Split(new[] { ' ' }, sso));
            }

            //Questions
            if (!string.IsNullOrWhiteSpace(request.Q1))
            {
                request.Q1 = string.Join("+", request.Q1.Split(new[] { ' ' }, sso));
            }
            if (!string.IsNullOrWhiteSpace(request.Q2))
            {
                request.Q2 = string.Join("+", request.Q2.Split(new[] { ' ' }, sso));
            }
            if (!string.IsNullOrWhiteSpace(request.Q3))
            {
                request.Q3 = string.Join("+", request.Q3.Split(new[] { ' ' }, sso));
            }
            if (!string.IsNullOrWhiteSpace(request.Q4))
            {
                request.Q4 = string.Join("+", request.Q4.Split(new[] { ' ' }, sso));
            }
            if (!string.IsNullOrWhiteSpace(request.Q6))
            {
                request.Q6 = string.Join("+", request.Q6.Split(new[] { ' ' }, sso));
            }
            if (!string.IsNullOrWhiteSpace(request.Q5))
            {
                request.Q5 = string.Join("+", request.Q5.Split(new[] { ' ' }, sso));
            }


            // Comments
            if (!string.IsNullOrWhiteSpace(request.Comments1))
            {
                request.Comments1 = string.Join("+", request.Comments1.Split(new[] { ' ' }, sso));
            }
        }
    }
}
