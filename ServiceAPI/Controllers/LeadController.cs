using CBR.Core.Logic.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using CBR.Core.Entities.ResourceModels;

namespace ServiceAPI.Controllers
{
    [RoutePrefix("api/leads")]
    public class LeadController : ApiController
    {
        [Route("coreg")]
        [HttpPost]
        public IHttpActionResult PostCoregLead(Lead coreglead)
        {
            try
            {
                var manager = new LeadManager();

                coreglead.Ip = Utility.GetClientIpAddress();

                //returns new lead with id
                return Ok(manager.AddOrUpdateCoregLead(coreglead));
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    return Ok(e.InnerException.Message);
                return Ok(e.Message);
            }

        }

        [Route("{emailAddress}/{offerId}")]
        [HttpGet]
        public IHttpActionResult GetLead(string emailAddress, string offerId, [FromUri] string affiliateId = null)
        {
            try
            {
                string clientAddress = HttpContext.Current.Request.UserHostAddress;

                var manager = new LeadManager();

                return Ok(manager.GetLead(emailAddress, offerId, affiliateId));
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                return Ok(e.InnerException.Message);
                return Ok(e.Message);
            }

        }
        [Route("country/{ip}")]
        [HttpGet]
        public IHttpActionResult GetCountry(string ip)
        {
            try
            {
                var manager = new LeadManager();

                return Ok(manager.GetCountry(ip));
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
