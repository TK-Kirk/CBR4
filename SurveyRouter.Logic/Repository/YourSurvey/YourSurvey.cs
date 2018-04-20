using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SurveyRouter.Logic.Data.Enums;
using SurveyRouter.Logic.Data.Models;
using SurveyRouter.Logic.Data.Models.YourSurveys;

namespace SurveyRouter.Logic.Repository.YourSurvey
{
    public class YourSurvey
    {
        public async Task<List<RouterReturn>> GetSurveys()
        {
            var result = await GetRawSurveys(new RouterUser());

            List<RouterReturn> returnlist = new List<RouterReturn>();
            foreach (var survey in result.surveys)
            {
                //returnlist.Add(new RouterReturn() { HostId = (int)RouterHost.YourSurvey, Name = survey.name, URL = survey.entry_link });
                returnlist.Add(new RouterReturn() { });
            }

            return returnlist;
        }

        public  async Task<Result> GetRawSurveys(RouterUser user)
        {
            var client = new HttpClient();
            // Update port # in the following line.
            client.BaseAddress = new Uri("https://www.your-surveys.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("x-yoursurveys-api-key", ConfigurationManager.AppSettings["YourSurveysSecretKey"]);

            string dobString = null;
            if (user.DOB != null)
            {
                dobString = user.DOB.Value.ToString("yyyy-MM-dd");
            }

            //"/suppliers_api/surveys/user?user_id=12&date_of_birth=1990-01-01&email=tk%40webhenmedia.com&gender=m&zip_code=68135&ip_address=68.225.172.87&limit=10&basic=1")


             var surveysReturnedLimit = ConfigurationManager.AppSettings["YourSurveysMaxReturned"];

            //var surveysReturnedLimit = "10";
            string uri = $"/suppliers_api/surveys/user?user_id={user.UniqueId}&date_of_birth={dobString}&email={user.Email}&gender={user.Gender}&zip_code={user.Zip}&ip_address={user.IpAddress}&limit={surveysReturnedLimit}&basic=1";
            HttpResponseMessage response = client.GetAsync(uri).Result;


            var content = await response.Content.ReadAsStringAsync();

            Result result = null;
            try
            {
                result = JsonConvert.DeserializeObject<Result>(content);
            }
            catch (Exception e)
            {
                result = new Result();
                result.status = "error";
                result.messages = new List<string>()
                {
                    $"Serialization Error From Your Surveys{Environment.NewLine}",
                    e.Message,
                    content
                }.ToArray();
                //Console.WriteLine(e);
                //throw;
            }

            return result;
        }
    }


}
