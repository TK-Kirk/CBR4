using CBR.Core.Entities.ExternalResouceModels.ProvideMedia;
using CBR.Core.Logic.Managers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ServiceAPI.Controllers
{
    [RoutePrefix("api/post")]
    public class PostController : ApiController
    {
        [Route("providemedia")]
        [HttpPost]
        public IHttpActionResult PostProvideMedia(ProvideMediaRequest request)
        {
            try { 
                var isTest = Properties.Settings.Default.ProvideMediaTest;



                //request.Contact.Ip = Utility.GetClientIpAddress();
                
                var postManager = new PostManagerProvideMedia();

                return Ok(postManager.SubmitProvideMediaLead(request, Utility.GetClientIpAddress(), isTest));
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    return Ok(e.InnerException.Message);
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

                return Ok(postManager.SubmitProvideMediaLead(request.RetryRequest, Utility.GetClientIpAddress(), isTest));
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
