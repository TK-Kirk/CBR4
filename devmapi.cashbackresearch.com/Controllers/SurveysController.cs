using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using CBR.Core.Entities.Models;
using CBR.Core.Logic.Managers;
using SurveyRouter.Logic.Data.Models;
using SurveyRouter.Logic.Logic;
using RouterContact = SurveyRouter.Logic.Data.RouterContact;
using RouterHost = SurveyRouter.Logic.Data.Enums.RouterHost;

namespace devmapi.cashbackresearch.com.Controllers
{
    [RoutePrefix("api/surveys")]
    public class SurveysController : ApiController
    {

        [HttpGet]
        [Route("SendSurveyEmail")]
        public IHttpActionResult SendSurveyEmail([FromUri]string emailAddress)
        {
            var manager = new RouterManager();
            var result = manager.SendSurveyEmail(emailAddress, Utility.GetClientIpAddress());
            return Ok(result);
        }


        [HttpGet]
        [Route("GetSurveysMobile")]
        public IHttpActionResult GetSurveysMobile([FromUri] string userId)
        {

            //return Ok(new Uri(Request.RequestUri, RequestContext.VirtualPathRoot));
            //returns 
            //http://devmapi.cashbackresearch.com/
            try
            {
                var usermanager = new MobileUserManager();
                
                Guid id =  usermanager.GetRouterContactIdFromMobileUser(Guid.Parse(userId));
                var manager = new RouterManager();
                RouterReturnContainer r = manager.GetUserSurveys(id, Utility.GetClientIpAddress());
                return Ok(r.RouterReturnList);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
                return InternalServerError(e);
            }
        }
        [HttpGet]
        [Route("GetSurveys")]
        public IHttpActionResult Get([FromUri] string userId)
        {

            //return Ok(new Uri(Request.RequestUri, RequestContext.VirtualPathRoot));
            //returns 
            //http://devmapi.cashbackresearch.com/
            try
            {
                var manager = new RouterManager();

                RouterReturnContainer r = manager.GetUserSurveys(Guid.Parse(userId), Utility.GetClientIpAddress());
                return Ok(r.RouterReturnList);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
                return InternalServerError(e);
            }
        }


        [HttpGet]
        [Route("SurveyAction")]
        public IHttpActionResult SurveyAction([FromUri]int routerContactId, [FromUri]int hostId, [FromUri]int projectId, [FromUri] string surveyURL)
        {
            //1. save record in RouterAction table
            //2. the transactionid needs to be appended to the url
            //3. the varible name that the transId gets assigned to varies by host

            //return Ok(new Uri(Request.RequestUri, RequestContext.VirtualPathRoot));
            //returns 
            //http://devmapi.cashbackresearch.com/

            try
            {

                //var compare = Utility.HashHmacMD5("unique_user_id=b1e269bc-99a5-4b96-a095-06a9cd56d446", ConfigurationManager.AppSettings["YourSurveysSecretKey"]);

                var _manager = new RouterManager();
                var routerAction =_manager.SetSurveyAction(routerContactId, hostId, projectId, surveyURL, Utility.GetClientIpAddress());


                //TODO append TransactionID. Maybe only on Your Surveys. 
                //TODO For precision sample, waiting to see if the transid comes back or we just have to use survey and userguid

                string newUrl = null;
                if (hostId == (int) RouterHost.YourSurvey)
                {
                    //surveyURL = $"{surveyURL}&supplier_sub_id={transationID}&supplier_sub_id2={routerContactId}";
                    surveyURL = $"{surveyURL}&ssi2={routerAction.TransactionId}&ssi3={routerContactId}";
                    //newUrl = CreateUrlForYourSurveys(surveyURL);
                }
                if (hostId == (int)RouterHost.PrecisionSample)
                {
                    surveyURL = $"{surveyURL}&sub_id={routerAction.TransactionId}";
                }

                routerAction.PostedUrl = surveyURL;
                _manager.SetRouterActionUrl(routerAction);


                return Redirect(surveyURL);
                //return Redirect(newUrl);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            //var response = actionContext.Request.CreateResponse(HttpStatusCode.Redirect);
            //response.Headers.Location = new Uri("https://www.stackoverflow.com");
            //actionContext.Response = response;
        }

        [HttpGet]
        [Route("RouterEmailPush")]
        public IHttpActionResult RouterEmailPush([FromUri] string emailAddress)
        {
            //this method takes an email address. Checks if minimum data exists. Then
            //if it doesn't it returns a flag to collect more data. 
            //if minumum data does exist it creates a new router user and redirects them ot
            //the daily surveys page
            var _manager = new RouterManager();
            MinimumInfoExistsResult minimumInfoExistsResult = _manager.CheckIfMinimumInfoExistsForEmail(emailAddress);

            if (minimumInfoExistsResult.HasMinimumInfo && !minimumInfoExistsResult.HasRounterContact)
            {
                RouterContact user = _manager.RouterContactFullSetup(emailAddress);
                minimumInfoExistsResult.HasRounterContact = true;
                minimumInfoExistsResult.RouterContactUniqueId = user.UniqueId;
            }

            return Ok(minimumInfoExistsResult);
        }

        [HttpPost]
        [Route("UpdateMinimumInfo")]
        public IHttpActionResult UpdateMinimumInfo([FromBody] MinimumSurveyInfo minimumSurveyInfo)
        {
            //second half of RouterEmailPush.
            //if all info not present then it gets sent in. 
            //then set up for surveys
            try
            {
                var _manager = new RouterManager();
                _manager.UpdateMinimumSurveyInfo(minimumSurveyInfo);
                RouterContact user = _manager.RouterContactFullSetup(minimumSurveyInfo.EmailAddress);
                return Ok(user.UniqueId);
            }
            catch (Exception e)
            {
                return Ok(e.Message);

            }
        }

        private string CreateUrlForYourSurveys(string surveyUrl)
        {
            string[] urlParsed = surveyUrl.Split('?');
            string newUrl = urlParsed[0] + "?";
            string querystring = "";
            string[] querystringArray = urlParsed[1].Split('&');
            string hmac = "";
            foreach (string s in querystringArray)
            {
                string[] values = s.Split('=');
                switch (values[0])
                {
                    case "si":
                    case "ssi":
                    case "ssi3":
                    case "offer_id":
                    case "lang":
                        querystring += "&" + s;
                        break;
                    case "hmac":
                        break;
                    default:
                        hmac += "&" + s;
                        querystring += "&" + s;
                        break;
                }

            }
           //hmac += $"&supplier_sub_id={transationId}&supplier_sub_id2={routerContactId}";

            var hmacHashed = Utility.HashHmacMD5(hmac.TrimStart('&'), ConfigurationManager.AppSettings["YourSurveysSecretKey"]);

            querystring = querystring.TrimStart('&');

            newUrl += querystring +  "&hmac=" + hmacHashed;

            return newUrl;
        }


        [Route("Postback/2")]
        [HttpGet]
        //Postback for Precision Samples
        public IHttpActionResult PostbackPrecisionSample([FromUri] string ug,
            string sub_id,
            decimal gross, decimal reward,
            string status,  DateTime date, int surveyid)
        {
            //http://devmapi.cashbackresearch.com/api/surveys/postback/2?ug=F1CA2526-4995-47A1-89BE-474A223505D9&sub_id=eeeeeeeeeeee&reward=1.2&status=S&date=1-1-2017&title=&surveyid=12345&gross=2.4

            try
            {
                var p = new PostbackManager();
                p.PostbackPrecisionSample(ug, sub_id, reward, status, date, surveyid, gross);

                return Ok();
            }
            catch (Exception e)
            {
                var msg = e;
                return InternalServerError();
            }
        }

        [Route("Redirect/2")]
        [HttpGet]
        //Redirect for Precision Samples
        public IHttpActionResult RedirectPrecisionSample([FromUri] string ug,
            string sub_id,
            decimal gross, decimal reward,
            string status,  DateTime date, int surveyid)
        {
        try
        {
                //test url
                //http://devmapi.cashbackresearch.com/api/surveys/postback/2?ug=F1CA2526-4995-47A1-89BE-474A223505D9
                var m = new RouterManager();

            var p = new PostbackManager();
            p.PostbackPrecisionSample(ug, sub_id, reward, status, date, surveyid, gross);

                var uniqueId = m.GetRouterUserFromPrecisionSampleId(ug);
            var url = $"{ConfigurationManager.AppSettings["BaseWebUrl"]}mysurveys?ug={uniqueId}";
            return Redirect(url);
            }
            catch (Exception e)
            {
                var msg = e;
                return InternalServerError();
            }
        }


        [Route("Redirect/1")]
        [HttpGet]
        //Redirect for Your Surveys
        public IHttpActionResult RedirectYourSurveys([FromUri] int ssi3)
        {
            try
            {
                var m = new RouterManager();
                var uniqueId = m.GetRouterContactUnqueId(ssi3);
                var url = $"{ConfigurationManager.AppSettings["BaseWebUrl"]}mysurveys?ug={uniqueId}";
                return Redirect(url);
            }
            catch (Exception e)
            {
                var msg = e;
                return InternalServerError();
            }
        }

        [Route("Postback/1")]
        [HttpGet]
        //Postback for your surveys
        public IHttpActionResult PostbackYourSurveys([FromUri] string ssi2, string ssi3, string ip, string transactionId, string signature_md5, string supplier_sub_id)
        {
            //http://devmapi.cashbackresearch.com/api/surveys/postback/1?ssi2=c5b6b137-2cf8-4ac7-99ce-44f4609d7002&ssi3=33476&ssi3=bbb&ip=68.225.172.87&transactionId=ccccc&signature_md5=ddddddddddd
            var p = new PostbackManager();


            //the md5 hash is OurTransactionId:transactionidYS:secretkey
            //params are supplier_sub_id:transaction_id(YS):secret_key.
            // we return the following to YourSurveys:
            //surveyURL = $"{surveyURL}&supplier_sub_id={transationID}&supplier_sub_id2={routerContactId}";
            // add key = "YourSurveysSecretKey" value = "0f75db394bd25ea21977e147a527be5e" 
            if (ConfigurationManager.AppSettings["CheckYourSurveysHash"] == "true")
            {
                //test 6AB71E5A-FF7B-4BB8-B93D-9D956F7BD67B:ccccc:0f75db394bd25ea21977e147a527be5e
                //test hashed = 4fc4941aed12393d5844529353768d22

                //test2 :ccccc:0f75db394bd25ea21977e147a527be5e
                //test2 hashed = f34692a4e347aabbabc6720c9773cfb6
                //test2 url http://devmapi.cashbackresearch.com/api/surveys/postback/1?ssi2=c5b6b137-2cf8-4ac7-99ce-44f4609d7002&ssi3=33476&ip=68.225.172.87&transactionId=ccccc&signature_md5=f34692a4e347aabbabc6720c9773cfb6
                //http://mapi.cashbackresearch.com/api/surveys/postback/1?ssi2=c5b6b137-2cf8-4ac7-99ce-44f4609d7002&ssi3=33476&ip=68.225.172.87&transactionId=ccccc&signature_md5=f34692a4e347aabbabc6720c9773cfb6

                //if (!p.VerifyYourSurveysHash(supplier_sub_id,transactionId, signature_md5 ))
                if (!p.VerifyYourSurveysHash(supplier_sub_id, transactionId, signature_md5 ))
                {
                    return BadRequest();
                };
            }

            try
            {
                p.PostbackYourSurveys(ssi2,ssi3,ip,transactionId,signature_md5);
                return Ok();
            }
            catch (Exception e)
            {
                var msg = e;
                return InternalServerError();

            }

        }

        // PUT: api/Surveys/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Surveys/5
        public void Delete(int id)
        {
        }
    }


}
