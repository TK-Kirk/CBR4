using CBR.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBR.Core.Entities.ExternalResouceModels.XVerify;
using CBR.Core.Entities.Models;
using CBR.DataAccess.Repositories;
using Newtonsoft.Json;

namespace CBR.Core.Logic.Managers
{
    public class XVerifyManager
    {
        public AddressVerificationResponse AddressInfo { get; }
        public IpVerificationResponse IpInfo { get; }
        private ZipIpVerifyResult _zipIpVerifyResult;

        GloshareContext _db;
        XVerifyRepository _xverifyRepository;

        public XVerifyManager()
        {
            _db = new GloshareContext();
            _xverifyRepository = new XVerifyRepository();
        }

        public ZipIpVerifyResult VerifyZipCode(string email, string ip,string street, string zip)
        {
            if (_db.CbrZipVerifieds.Any(z => z.EmailAddress == email && z.ValidZip == zip && z.ValidIpAddress == ip)){
                return new ZipIpVerifyResult() { IsValid = true };
            }

            var xverifyZipResult = Validate(ip, street, zip, email);
            if (xverifyZipResult.IsValid)
            {
                return new ZipIpVerifyResult() { IsValid = true };
            }

            LogInvalidEntry(ip, street, zip, email);
            return xverifyZipResult;
        }

        private void LogInvalidEntry(string ip, string street, string zip, string email)
        {
            string invalidIpJson = JsonConvert.SerializeObject(IpInfo);
            string invalidAddressJson = JsonConvert.SerializeObject(AddressInfo);
            _db.VerifyZipFailures.Add(new VerifyZipFailure()
            {
                EmailAddress = email,
                Street = street,
                Zip = zip,
                IpAddress = ip,
                NoMatch = _zipIpVerifyResult.NoMatch,
                InValidAddress = _zipIpVerifyResult.AddressInvalid,
                InvalidIp = _zipIpVerifyResult.IpInvalid,
                InvalidZip = _zipIpVerifyResult.ZipCodeInvalid,
                AddressVerifyResultJson = invalidAddressJson,
                IpVerifyResultJson = invalidIpJson
            });
            _db.SaveChanges();
        }

        private ZipIpVerifyResult Validate(string ip, string street, string zip, string email)
        {
            
            IpVerificationResponse ipInfo = _xverifyRepository.GetIpVerification(ip);
            AddressVerificationResponse addressInfo = _xverifyRepository.GetAddressVerification(street, zip);

            if (ipInfo.IsValid && addressInfo.IsValid)
            {
                if (ipInfo.ipdata.region == addressInfo.address.state)
                {
                    _db.CbrZipVerifieds.Add(new CbrZipVerified()
                    {
                        EmailAddress = email,
                        ValidIpAddress = ip,
                        ValidZip = zip
                    });
                    _db.SaveChanges();
                    return new ZipIpVerifyResult() { IsValid = true};
                    //return true;
                }

                return new ZipIpVerifyResult() {IsValid = false, NoMatch = true};
                //return false;
            }

            _zipIpVerifyResult = new ZipIpVerifyResult() {IsValid = false, Message = ""};

            if (!addressInfo.IsValid)
            {
                if (addressInfo.address.message.Contains("Zip Code"))
                {
                    _zipIpVerifyResult.ZipCodeInvalid = true;
                    _zipIpVerifyResult.Message += addressInfo.address.message;
                }
                else
                {
                    _zipIpVerifyResult.AddressInvalid = true;
                    _zipIpVerifyResult.Message += addressInfo.address.message;
                }
            }

            if (!ipInfo.IsValid)
            {
                _zipIpVerifyResult.IpInvalid = true;
                _zipIpVerifyResult.Message += ipInfo.ipdata.message;
            }

            return _zipIpVerifyResult;
        }

        public class ZipIpVerifyResult
        {
            public bool IsValid { get; set; }
            public bool ZipCodeInvalid { get; set; }
            public bool AddressInvalid { get; set; }
            public bool IpInvalid { get; set; }
            public bool NoMatch { get; set; }
            public string Message { get; set; }
        }

        public AddressVerificationResponse GetAddressVerification(string streeet, string zip)
        {
            return _xverifyRepository.GetAddressVerification(streeet, zip);
        }
    }
    
}

