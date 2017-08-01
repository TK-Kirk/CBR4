using CBR.Core.Entities.ExternalResouceModels.ProvideMedia;
using CBR.Core.Logic.Managers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CBR.Core.Entities.ExternalResouceModels.EngageIQ;

namespace ServiceAPI.Controllers
{
    [RoutePrefix("api/post")]
    public class PostController : ApiController
    {
        [Route("engageiq")]
        [HttpPost]
        public IHttpActionResult PostEngageIq(EngageIqRequest request)
        {
            try
            {
                string clientIpAddress = Utility.GetClientIpAddress();

                var postManager = new PostManagerEngageIQ();
                return Ok(postManager.SubmitLead(request, clientIpAddress));
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    return Ok(e.InnerException.Message);
                return Ok(e.Message);
            }
        }

        [Route("providemedia")]
        [HttpPost]
        public IHttpActionResult PostProvideMedia(ProvideMediaRequest request)
        {
            try { 
                var isTest = Properties.Settings.Default.ProvideMediaTest;
                var postManager = new PostManagerProvideMedia();
                return Ok(postManager.SubmitLead(request, Utility.GetClientIpAddress(), isTest));
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    if (e.InnerException.InnerException != null)
                    {
                        return Ok(e.InnerException.InnerException.Message);
                    }
                    return Ok(e.InnerException.Message);
                }
               
                return Ok(e.Message);

            }
        }


        [Route("providemediaUpdate")]
        [HttpPost]
        public IHttpActionResult PostProvideMediaUpdate(ProvideMediaUpdateRequest request)
        {
            try
            {
                var isTest = Properties.Settings.Default.ProvideMediaTest;
                
                var postManager = new PostManagerProvideMedia();

                postManager.UpdateLeadWithProvideMediaData(request);

                return Ok(postManager.SubmitLead(request.RetryRequest, Utility.GetClientIpAddress(), isTest));
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    return Ok(e.InnerException.Message);
                return Ok(e.Message);
            }
        }

    }
}
