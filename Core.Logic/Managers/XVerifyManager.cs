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
        public AddressVerificationResponse AddressInfo { get; set; }
        public IpVerificationResponse IpInfo { get; set; }
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

            _zipIpVerifyResult = Validate(ip, street, zip, email);
            if (_zipIpVerifyResult.IsValid)
            {
                return new ZipIpVerifyResult() { IsValid = true };
            }

            LogInvalidEntry(ip, street, zip, email);
            return _zipIpVerifyResult;
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
            
            IpInfo = _xverifyRepository.GetIpVerification(ip);
            AddressInfo = _xverifyRepository.GetAddressVerification(street, zip);

            if (IpInfo.IsValid && AddressInfo.IsValid)
            {
                if (IpInfo.ipdata.region == AddressInfo.address.state)
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

            if (!AddressInfo.IsValid)
            {
                if (AddressInfo.address.message.Contains("Zip Code"))
                {
                    _zipIpVerifyResult.ZipCodeInvalid = true;
                    _zipIpVerifyResult.Message += AddressInfo.address.message;
                }
                else
                {
                    _zipIpVerifyResult.AddressInvalid = true;
                    _zipIpVerifyResult.Message += AddressInfo.address.message;
                }
            }

            if (!AddressInfo.IsValid)
            {
                _zipIpVerifyResult.IpInvalid = true;
                _zipIpVerifyResult.Message += IpInfo.ipdata.message;
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

