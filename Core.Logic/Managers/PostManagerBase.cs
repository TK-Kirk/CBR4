using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using CBR.Core.Entities.ResourceModels;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using CBR.Core.Entities.Models;
using CBR.DataAccess;
using System.Web;
using CBR.Core.Entities.ExternalResouceModels;


namespace CBR.Core.Logic.Managers
{

    public class PostManagerBase
    {
        protected string Get(string url)
        {
            // Create a request using a URL that can receive a post. 
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";

            WebResponse response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            //Console.WriteLine (responseFromServer);
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
        }

        protected GloshareContext _db = new GloshareContext();
        protected string Post(string postData, string url)
        {
            // Create a request using a URL that can receive a post. 
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            //string postData = "This is a test that posts this string to a Web server.";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            //Console.WriteLine (responseFromServer);
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
        }


        protected string GetAge(DateTime? birthDate)
        {
            if (birthDate.HasValue)
            {
                var today = DateTime.Today;
                // Calculate the age.
                var age = today.Year - birthDate.Value.Year;
                // Go back to the year the person was born in case of a leap year
                if (birthDate > today.AddYears(-age)) age--;
                return age.ToString();
            }
            return null;
        }

        protected void EncodeContact(Lead contact)
        {
            HttpContext ctx = HttpContext.Current;

            contact.Email = ctx.Server.HtmlEncode(contact.Email);
            contact.Firstname = ctx.Server.HtmlEncode(contact.Firstname);
            contact.Lastname = ctx.Server.HtmlEncode(contact.Lastname);
            contact.Address = ctx.Server.HtmlEncode(contact.Address);
            contact.Gender = ctx.Server.HtmlEncode(contact.Gender);
            contact.Phone = ctx.Server.HtmlEncode(contact.Phone);
            contact.City = ctx.Server.HtmlEncode(contact.City);
            contact.State = ctx.Server.HtmlEncode(contact.State);
            contact.Zip = ctx.Server.HtmlEncode(contact.Zip);
            contact.Ip = ctx.Server.HtmlEncode(contact.Ip);
        }

        protected string PreparePhoneForPost(string phone)
        {
            string fixedPhone = null;
            if (phone != null)
            {
                Regex digitsOnly = new Regex(@"[^\d]");
                fixedPhone = digitsOnly.Replace(phone, "");
                if (fixedPhone.Length == 10)
                {
                    //format  (917) 478-3878
                    fixedPhone = string.Format("({0}) {1}-{2}", fixedPhone.Substring(0, 3), fixedPhone.Substring(3, 3), fixedPhone.Substring(6, 4));
                }
            }

            return fixedPhone;
        }


        protected void WriteCoregError(string partner, string postdata, string url, string response)
        {
            GloshareContext _db = new GloshareContext();

            _db.CoregErrors.Add(new CoregError()
            {
                DateInserted = DateTime.Now,
                Partner = partner,
                PostData = postdata,
                Url = url,
                Response = response
            });
            _db.SaveChanges();
        }

        protected string GenderGetSingleCharacter(CbrLead lead)
        {
            var gender = "";
            if (!string.IsNullOrWhiteSpace(lead.Gender))
            {
                gender = lead.Gender.Substring(0, 1).ToUpper();
            }
            return gender;
        }


        protected void UpdateLeadAccepted(CoregPostRequestBase postRequest, CbrLead lead)
        {
            var campaign =  _db.CoregCampaigns.First(c => c.CoregCampaignId == (int) postRequest.CampaignCodeId);

            _db.CoregLeadAccepteds.Add(new CoregLeadAccepted()
            {
                CbrLeadId = lead.CbrLeadId,
                CoregCampaignId = (int) postRequest.CampaignCodeId,
                CoregPartnerId = campaign.CoregPartnerId
            });

            _db.SaveChanges();
        }

        protected void UpdateLeadCityStateIP(XVerifyManager xvm, CbrLead lead, string ipAddress)
        {
            //Update state/city/ip 
            if (string.IsNullOrWhiteSpace(lead.City) || string.IsNullOrWhiteSpace(lead.State))
            {
                //used cached xverify info if available
                if (xvm.IpInfo?.ipdata != null)
                {
                    lead.City = xvm.IpInfo.ipdata.city;
                    lead.State = xvm.IpInfo.ipdata.region;
                }
                else
                {
                    var ipInfo = xvm.GetIpVerification(ipAddress);
                    lead.City = ipInfo.ipdata.city;
                    lead.State = ipInfo.ipdata.region;
                }
                _db.SaveChanges();
            }

            if (lead.Ip != ipAddress)
            {
                lead.Ip = ipAddress;
                _db.SaveChanges();
            }
        }

        protected bool IsGreaterThan(string value, int testValue)
        {
            int intValue;
            if (int.TryParse(value, out intValue))
            {
                return intValue > testValue;
            }
            return false;
        }

    }
}
