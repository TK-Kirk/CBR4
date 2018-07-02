using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using CBR.Core.Entities.ResourceModels;
using CBR.Core.Logic.Managers;
using SurveyRouter.Logic.Logic;

namespace devmapi.cashbackresearch.com.Controllers
{
    [RoutePrefix("api/users")]

    public class UsersController : ApiController
    {
        // GET: api/Users/48668DD9-0CCE-4C98-AE33-F920718E0C79
        //[Route("{userId}")]
        public IHttpActionResult Get(Guid id)
        {
            var m = new MobileUserManager();
            UserRegistrationRequest u = m.GetUser(id);
            return Ok(u);
        }
        public string Get()
        {
            //var m = new MobileUserManager();


            //var u = m.GetUser(Guid.Parse(userId));
            //return Ok(u);
            return "yep2";
        }

        //POST: api/Users
        [Route("signup")]
        [HttpPost]
        public IHttpActionResult Post([FromBody]string email)
        {
            var m = new MobileUserManager();
            var result = m.SignUpUser(email, Utility.GetClientIpAddress());
            return Ok(result);

        }

        [Route("earnings")]
        [HttpGet]
        public IHttpActionResult GetEarnings([FromUri] Guid userId)
        {
            var m = new MobileUserManager();
            decimal earnings = m.GetUserEarnings(userId);
            return Ok(earnings);
        }
        // PUT: api/Users/5
        [Route("registration")]
        [HttpPost]
        public IHttpActionResult UpdateRegistration([FromUri] Guid userId, [FromBody]UserRegistrationRequest userRegistrationRequest)
        {
            if (!string.IsNullOrWhiteSpace(userRegistrationRequest.Zip))
            {
                XVerifyManager x = new XVerifyManager();
                var zipinfo = x.GetZipcodeLookup(userRegistrationRequest.Zip);
                if (!zipinfo.IsValid)
                {
                    return Ok(new UserRegistrationResponse()
                    {
                        ZipIsValid = false,
                        Success = false,
                        Message = "Zipcode is invalid."
                    });
                }
                userRegistrationRequest.State = zipinfo.zipdata.state;
                userRegistrationRequest.City = zipinfo.zipdata.primary_city;
            }

            var m = new MobileUserManager();
            int? routerContactId = m.Register(userId, userRegistrationRequest);

            if (!routerContactId.HasValue)
            {
                var rm = new RouterManager();
                var routerContact = rm.RouterContactFullSetup(userRegistrationRequest.EmailAddress);
                routerContactId = routerContact.RouterContactId;
                m.SetRouterContact(routerContactId, userId);
            }

            return Ok(new UserRegistrationResponse()
            {
                ZipIsValid = true,
                Success = true,
                Message = "Success."
            });

        }




        //// DELETE: api/Users/5
        //public void Delete(int id)
        //{
        //}
    }
}
