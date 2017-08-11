using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Admin.Logic.Managers;

namespace AdminServiceAPI.Controllers
{
    [RoutePrefix("api/coreg")]

    public class CoregController : ApiController
    {
        [HttpGet]
        [Route("campaigns")]
        public IHttpActionResult GetCampaigns()
        {
            var _manager = new CoregManager();
            return Ok(_manager.GetCampaigns());
        }

        [HttpPut]
        [Route("campaigns/{id}/active/{active}")]
        public IHttpActionResult GetCampaigns(int id, bool active)
        {
            var _manager = new CoregManager();
            return Ok(_manager.SetCampaignActive(id, active));
        }
    }
}
