using CBR.Core.Logic.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using CBR.Core.Entities.ResourceModels;
using CBR.DataAccess.Repositories;
using CBR.Core.Entities.ExternalResouceModels.Listrak;

namespace ServiceAPI.Controllers
{
    [RoutePrefix("api/listrak")]
    public class ListController : ApiController
    {
        [Route("coreg")]
        [HttpPost]
        public IHttpActionResult PostCoregLead(CoregPostRequest request)
        {
            try
            {
                var listrak = new ListrakRepository();

                return Ok(listrak.UpdateLists(request.Data, request.ListsToUpdate, request.PageName));
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    return Ok(e.InnerException.Message);
                return Ok(e.Message);
            }

        }

        [Route("addhocUpdate")]
        [HttpPost]
        public IHttpActionResult PostAdhoc(AdHocRequest request)
        {
            try
            {

                var listrak = new ListrakRepository();
                return Ok(listrak.UpdateLists(request.FieldItems, request.ListsToUpdate, request.PageName));

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
