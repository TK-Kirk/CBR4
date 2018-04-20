using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SurveyRouter.Logic.Data.Models;

namespace SurveyRouter.Logic.Repository.Listrak
{
    public class ListrakRest
    {
        HttpClient _client;

        private enum DaileySurveyFields
        {
            FromAddress = 2442835,
            FromName = 2442836,
            Subject = 2442838,
            TextContent = 2442839,
            HTMLContent = 2442837,
            surveylink1 = 2442994,
            surveylink2 = 2442995,
            surveylink3 = 2442996,
            surveyname1 = 2442967,
            surveyname2 = 2442968,
            surveyname3 = 2442969,
            date = 2442970,
            time = 2442971,
            dailysurveyslink = 2442972,
            surveystats1 = 2442973,
            surveystats2 = 2442974,
            surveystats3 = 2442975,
            surveyreward1 = 2442976,
            firstname = 2442977,
            surveyquantity = 2442978
        }

        public bool SendDailySurveysEmail(RouterUser user, List<RouterReturn> surveys, out string message)
        {

            MessageEventBody body = CreateMessageBody(user, surveys);
            Uri uri = new Uri("https://api.listrak.com/email/v1/List/248553/Contact?eventIds=12598");
            var response = _client.PostAsJsonAsync(uri, body).Result;
            var result = response.Content.ReadAsStringAsync().Result;

            message = result;

            MailSendReturnStatus status = JsonConvert.DeserializeObject<MailSendReturnStatus>(result);

            if (status.status == 200 || status.status == 201)
            {
                return true;
            }

            return false;
        }

        private MessageEventBody CreateMessageBody(RouterUser user, List<RouterReturn> surveys)
        {
            RouterReturn survey1 = surveys.First();
            RouterReturn survey2 = surveys.Skip(1).Take(1).First();
            RouterReturn survey3 = surveys.Skip(2).Take(1).First();

            var survey1Reward = survey1.SubTitle.Split('*').ToList().Last();

            MessageEventBody body = new MessageEventBody();

            body.emailAddress = user.Email;
            body.subscriptionState = "Subscribed";
            body.segmentationFieldValues = new Segmentationfieldvalue[]
            {
                new Segmentationfieldvalue(){ segmentationFieldId = (int)DaileySurveyFields.firstname, value = user.First},
                new Segmentationfieldvalue(){ segmentationFieldId = (int)DaileySurveyFields.surveyreward1, value = survey1Reward },

                new Segmentationfieldvalue(){ segmentationFieldId = (int)DaileySurveyFields.surveylink1, value = survey1.ProxyUrl },
                new Segmentationfieldvalue(){ segmentationFieldId = (int)DaileySurveyFields.surveyname1, value = survey1.Title },
                new Segmentationfieldvalue(){ segmentationFieldId = (int)DaileySurveyFields.surveystats1, value = survey1.SubTitle},

                new Segmentationfieldvalue(){ segmentationFieldId = (int)DaileySurveyFields.surveylink2, value = survey2.ProxyUrl },
                new Segmentationfieldvalue(){ segmentationFieldId = (int)DaileySurveyFields.surveyname2, value = survey2.Title },
                new Segmentationfieldvalue(){ segmentationFieldId = (int)DaileySurveyFields.surveystats2, value = survey2.SubTitle},

                new Segmentationfieldvalue(){ segmentationFieldId = (int)DaileySurveyFields.surveylink3, value = survey3.ProxyUrl },
                new Segmentationfieldvalue(){ segmentationFieldId = (int)DaileySurveyFields.surveyname3, value = survey3.Title },
                new Segmentationfieldvalue(){ segmentationFieldId = (int)DaileySurveyFields.surveystats3, value = survey3.SubTitle},

                new Segmentationfieldvalue(){ segmentationFieldId = (int)DaileySurveyFields.dailysurveyslink, value = $"{ConfigurationManager.AppSettings["DailySurveysLink"]}{user.UniqueId}"},

                new Segmentationfieldvalue(){ segmentationFieldId = (int)DaileySurveyFields.surveyquantity, value = (surveys.Count - 1).ToString() },
                new Segmentationfieldvalue(){ segmentationFieldId = (int)DaileySurveyFields.date, value = DateTime.Today.ToLongDateString()},
                new Segmentationfieldvalue(){ segmentationFieldId = (int)DaileySurveyFields.time, value = $"{DateTime.Now:hh:mm tt}"}
            };

            return body;
        }


        public ListrakRest()
        {
            _client = new HttpClient();
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            nvc.Add(new KeyValuePair<string, string>("client_id", ConfigurationManager.AppSettings["ListrakClientID"]));
            nvc.Add(new KeyValuePair<string, string>("client_secret", ConfigurationManager.AppSettings["ListrakClientSecret"]));
            var client = new HttpClient();
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, "https://auth.listrak.com/OAuth2/Token") { Content = new FormUrlEncodedContent(nvc) };
            HttpResponseMessage res = client.SendAsync(req).Result;

            var content = res.Content.ReadAsStringAsync().Result;

            Token result = JsonConvert.DeserializeObject<Token>(content);

            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + result.access_token);

        }

        public void SendDailySurveysEmail()
        {

        }

        internal class Token
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
        }

        public class MailSendReturnStatus
        {
            public int status { get; set; }
            public string resourceId { get; set; }
        }



        public class MessageEventBody
        {
            public string emailAddress { get; set; }
            public string subscriptionState { get; set; }
            public Segmentationfieldvalue[] segmentationFieldValues { get; set; }
        }

        public class Segmentationfieldvalue
        {
            public int segmentationFieldId { get; set; }
            public string value { get; set; }
        }


    }
}
