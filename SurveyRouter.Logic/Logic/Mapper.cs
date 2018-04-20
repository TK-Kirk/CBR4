using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SurveyRouter.Logic.Data.Enums;
using SurveyRouter.Logic.Data.Models;
using SurveyRouter.Logic.Data.Models.PrecisionSample;
using SurveyRouter.Logic.Data.Models.YourSurveys;

namespace SurveyRouter.Logic.Logic
{
    public class Mapper
    {
        public static List<RouterReturn> Map(Result yourSurveys, RouterUser user)
        {
            //this is mapping the Your Surveys Raw Result to 
            //the RouterReturn

            var returnList = new List<RouterReturn>();
            if (yourSurveys?.surveys == null || !yourSurveys.surveys.Any())
            {
                return returnList;
            }

            return yourSurveys.surveys.Select(s =>
                new RouterReturn()
                {
                    Title = ParseSurveyTitle(s),
                    SubTitle = ParseSubTitle(s),
                    //loi can be zero so don't divide by 0
                    EarningPotential = s.loi == 0 ? 0 : (s.conversion_rate * 100) * (s.cpi * 2) / s.loi,
                    ProxyUrl = GetProxyUrl(user.RouterContactId, RouterHost.YourSurvey, s.project_id, s.entry_link)
                })?.ToList();
            //return Ok(new Uri(Request.RequestUri, RequestContext.VirtualPathRoot));
            //returns 
            //http://devmapi.cashbackresearch.com/

            //http://devmapi.cashbackresearch.com/api/surveys/SurveyAction?routerContactId=1&hostId=1&projectId=1&surveyUrl=https%3A%2F%2Fwww.your-surveys.com%3Fsi%3D339%26ssi%3DSUBID%26unique_user_id%3D12%26hmac%3D5507767dbc1b2cc4514528c6e47db11e%26offer_id%3D10572805
        }

        private static string ParseSubTitle(Survey survey)
        {
            string surveyTime = $"{survey.loi} minutes";
            int successRate = (int)(survey.conversion_rate * 100);
            var result = $"{surveyTime} * {successRate}% success * ${Math.Round(survey.cpi / 2, 2)}";
            return result;
        }


        private static string ParseSurveyTitle(Survey survey)
        {
            string title = survey.name;
            if (survey.name.Contains("minutes"))
            {
                //remove the minutes text
                int index = survey.name.LastIndexOf("(");
                title = survey.name.Substring(0, index).Trim();
            }

            return title;
        }

        private static string ParseYourSurveysName(Survey survey)
        {
            try
            {
                string surveyTime = $"({survey.loi} minutes)";
                string name = survey.name.Replace(surveyTime, "");
                int successRate = (int)(survey.conversion_rate * 100);
                return $"{name} {successRate}% success * ${surveyTime}";
            }
            catch (Exception)
            {
                return survey.name;
            }
        }

        private static string GetProxyUrl(int userRouterContactId, RouterHost host, int projectId, string url)
        {
            var encodedURL = HttpUtility.UrlEncode(url);
            var baseApiURl = ConfigurationManager.AppSettings["BaseApiUrl"];
            var proxyUrl = 
                $"{baseApiURl}api/surveys/SurveyAction?routerContactId={userRouterContactId}&hostId={(int)host}&projectId={projectId}&surveyUrl={encodedURL}";
            return proxyUrl;
        }

        internal static IEnumerable<RouterReturn> Map(Surveys psSurveys, RouterUser user)
        {
            //this is mapping the Prescision Sample Raw Result to 
            //the RouterReturn

            var returnList = new List<RouterReturn>();
            if (psSurveys?.Survey == null || !psSurveys.Survey.Any())
            {
                return returnList;
            }

            return psSurveys.Survey.Select(s =>
                new RouterReturn()
                {
                    //HostId = (int)RouterHost.PrecisionSample,
                    //Name = $"{s.SurveyName}. {s.IR}% success rate ({s.SurveyLength} minutes)",
                    Title = s.SurveyName,
                    SubTitle = $"{s.SurveyLength} minutes * {s.IR}% success * ${s.RewardValue}",
                    //URL = s.SurveyUrl,
                    //SuccessRate = s.IR,
                    EarningPotential = s.IR * s.RewardValue / s.SurveyLength,
                    //ProjectId = s.ProjectId,
                    ProxyUrl = GetProxyUrl(user.RouterContactId, RouterHost.PrecisionSample, s.ProjectId, s.SurveyUrl)
                })?.ToList();
        }
    }
}
