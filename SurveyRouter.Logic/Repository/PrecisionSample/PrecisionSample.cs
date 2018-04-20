using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SurveyRouter.Logic.Data;
using SurveyRouter.Logic.Data.Models;
using SurveyRouter.Logic.Data.Models.PrecisionSample;
using RouterHost = SurveyRouter.Logic.Data.Enums.RouterHost;

namespace SurveyRouter.Logic.Repository.PrecisionSample
{
    public class PrecisionSample
    {
        public async Task<List<RouterReturn>> GetSurveys()
        {
            var surveyResult = await GetRawSurveys(new RouterUser());

            List<RouterReturn> returnlist = new List<RouterReturn>();
            foreach (var survey in surveyResult.Survey)
            {
                //returnlist.Add(new RouterReturn() { HostId = (int) RouterHost.PrecisionSample, URL = survey.SurveyUrl, Name =  $"{survey.SurveyName} ({survey.SurveyLength} minutes)" });
                returnlist.Add(new RouterReturn() { });
            }

            return returnlist;
        }

        public  async Task<Surveys> GetRawSurveys(RouterUser user)
        {
            var client = new HttpClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("UserGuid", user.PrecisionSampleUserID.ToString())
            });

            //http://api2.precisionsample.com/v2.asmx/GetSurveys
            var result = client.PostAsync("http://api2.precisionsample.com/v2.asmx/GetSurveys", content).Result;

            XmlSerializer serializer = new XmlSerializer(typeof(string), "http://tempuri.org/");

            string s = null;


            using (Stream stream = await result.Content.ReadAsStreamAsync())
            {
                s = (string) serializer.Deserialize(stream);
            }

            if (s == "<Surveys><Survey>No Surveys were found for your profile.</Survey></Surveys>")
            {
                return new Surveys(){ Survey = new SurveysSurvey[0]};
            }

            var innerSerializer = new XmlSerializer(typeof(Surveys));
            Surveys surveyResult = null;

            using (TextReader reader = new StringReader(s))
            {
                surveyResult = (Surveys) innerSerializer.Deserialize(reader);
            }

            return surveyResult;
        }

        public string CreateUser(RouterContact routerContact, FormUrlEncodedContent content, out bool error)
        {
            error = false;   
            var client = new HttpClient();

            //FormUrlEncodedContent content = CreatePostContent(routerContact, lead);
             
            var result = client.PostAsync("http://api2.precisionsample.com/v2.asmx/Create", content).Result;

            XmlSerializer serializer = new XmlSerializer(typeof(string), "http://tempuri.org/");
            string s = null;
            using (Stream stream =  result.Content.ReadAsStreamAsync().Result)
            {
                s = (string)serializer.Deserialize(stream);
            }
            XmlSerializer innerSerializer = new XmlSerializer(typeof(result));
            result surveyResult = null;

            using (TextReader reader = new StringReader(s))
            {
                try
                {
                    surveyResult = (result)innerSerializer.Deserialize(reader);

                    return surveyResult.UserGuid;
                }
                catch (Exception e)
                {
                    error = true;
                    return s; //return the raw content
                }
            }

            return null;
        }

        public FormUrlEncodedContent CreatePostContent(RouterContact routerContact, OptInLead lead)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("RID", "20690"),
                new KeyValuePair<string, string>("TxId", ""),
                new KeyValuePair<string, string>("ExtMemberId", routerContact.RouterContactId.ToString()),
                new KeyValuePair<string, string>("Country", lead.CountryId ?? "US"),
                new KeyValuePair<string, string>("State", lead.State),
                new KeyValuePair<string, string>("FirstName", lead.Firstname),
                new KeyValuePair<string, string>("LastName", lead.Lastname),
                new KeyValuePair<string, string>("EmailAddress", lead.EmailAddress),
                new KeyValuePair<string, string>("Zip", lead.Zip),
                new KeyValuePair<string, string>("Gender", GetGender(lead)),
                new KeyValuePair<string, string>("Dob", GetDOB(lead)),
                new KeyValuePair<string, string>("Address1", lead.Address),
                new KeyValuePair<string, string>("Address2", lead.Address2),
                new KeyValuePair<string, string>("City", lead.City),
                new KeyValuePair<string, string>("Ethnicity", GetEthnicity(lead)),
            });
            return content;
        }
        public FormUrlEncodedContent CreatePostContent(RouterContact routerContact, MobilelLead lead)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("RID", "20690"),
                new KeyValuePair<string, string>("TxId", ""),
                new KeyValuePair<string, string>("ExtMemberId", routerContact.RouterContactId.ToString()),
                new KeyValuePair<string, string>("Country", lead.CountryId ?? "US"),
                new KeyValuePair<string, string>("State", lead.State),
                new KeyValuePair<string, string>("FirstName", lead.Firstname),
                new KeyValuePair<string, string>("LastName", lead.Lastname),
                new KeyValuePair<string, string>("EmailAddress", lead.EmailAddress),
                new KeyValuePair<string, string>("Zip", lead.Zip),
                new KeyValuePair<string, string>("Gender", lead.Gender),
                new KeyValuePair<string, string>("Dob", lead.Dob?.ToString("MM/dd/yyyy")),
                new KeyValuePair<string, string>("Address1", lead.Address),
                new KeyValuePair<string, string>("Address2", null),
                new KeyValuePair<string, string>("City", lead.City),
                new KeyValuePair<string, string>("Ethnicity", null),
            });
            return content;
        }

        private string GetDOB(OptInLead lead)
        {
            if (lead.BirthdayDay.HasValue && lead.BirthdayDay > 0 && lead.BirthdayMonth.HasValue && lead.BirthdayMonth > 0 && lead.BirthdayYear.HasValue && lead.BirthdayYear.HasValue)
            {
                return $"{lead.BirthdayMonth}/{lead.BirthdayDay}/{lead.BirthdayYear}";
            }

            return null;
        }

        private string GetEthnicity(OptInLead lead)
        {
            if (string.IsNullOrWhiteSpace(lead.Ethnicity))
            {
                return null;
            }

            if (lead.Ethnicity == "Caucasian")
            {
                return "1";
            }
            if (lead.Ethnicity == "African American")
            {
                return "2";
            }
            if (lead.Ethnicity == "Hispanic")
            {
                return "3";
            }
            if (lead.Ethnicity == "Asian" || lead.Ethnicity == "Indian")
            {
                return "5";
            }
            if (lead.Ethnicity == "Native American")
            {
                return "4";
            }
            if (lead.Ethnicity == "Middle Eastern")
            {
                return "12";
            }
            if (lead.Ethnicity == "Other")
            {
                return "15";
            }

            return "15";
            
        }

        private string GetGender(OptInLead lead)
        {
            if (string.IsNullOrWhiteSpace(lead.Gender))
            {
                return null;
            }

            if (lead.Gender.Length == 1)
            {
                return lead.Gender;
            }

            return lead.Gender.Substring(0, 1);
        }
    }
}
