using CBR.Core.Logic.Managers;
using CBR.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ServiceAPI.Controllers
{
    [RoutePrefix("api/xverify")]
    public class XVerifyController : ApiController
    {
        private XVerifyRepository _xverifyRepository;

        public XVerifyController()
        {
            _xverifyRepository = new XVerifyRepository();
        }

        [Route("verifyemail/{email}")]
        [HttpGet]
        public IHttpActionResult VerifyEmail( string email)
        {
            try
            {

                return Ok(_xverifyRepository.GetEmailVerification(email));
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    return Ok(e.InnerException.Message);
                return Ok(e.Message);
            }

        }

        [Route("verifyaddress/{street}/{zip}")]
        [HttpGet]
        public IHttpActionResult VerifyAddress(string street, string zip)
        {
            try
            {
                return Ok(_xverifyRepository.GetAddressVerification(street, zip));
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    return Ok(e.InnerException.Message);
                return Ok(e.Message);
            }

        }


        [Route("verifyip/{ip}")]
        [HttpGet]
        public IHttpActionResult VerifyIP(string ip)
        {
            try
            {
                return Ok(_xverifyRepository.GetIpVerification(ip));
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
