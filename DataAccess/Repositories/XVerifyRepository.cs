using CBR.Core.Entities.ExternalResouceModels.XVerify;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBR.DataAccess.Repositories
{
    public class XVerifyRepository : BaseHttpClient
    {
        const string baseurl = "http://www.xverify.com/services/";
        const string key = "&type=json&apikey=1000346-1ELUPXVV&domain=cashbackresearch.com";
        public XVerifyRepository():base(baseurl)
        {

        }

        public ZipcodeLookupResponse GetZipcodeLookup(string zipcode)
        {
            string url = $"zipcode/lookup/?zipcode={zipcode}{key}";

            var response = _client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                string answer = response.Content.ReadAsStringAsync().Result;
                var r = JsonConvert.DeserializeObject<ZipcodeLookupResponse>(answer);
                return r;
            }

            return new ZipcodeLookupResponse();
        }
        public bool GetEmailVerification(string email)
        {
            string url = $"emails/verify/?email={email}{key}";

            var response = _client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                string answer = response.Content.ReadAsStringAsync().Result;
                var r = JsonConvert.DeserializeObject<EmailVerificationResponse>(answer);
                return r.IsValid;
            }

            return false;
        }

        public AddressVerificationResponse GetAddressVerification(string street, string zip)
        {
            string url = $"address/verify/?street={street}&zip={zip}{key}";

            var response = _client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                string answer = response.Content.ReadAsStringAsync().Result;
                var r = JsonConvert.DeserializeObject<AddressVerificationResponse>(answer);
                return r;
            }

            return new AddressVerificationResponse();
        }

        public IpVerificationResponse GetIpVerification(string ip)
        {
            string url = $"ipdata/verify/?ip={ip}{key}";

            var response = _client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                string answer = response.Content.ReadAsStringAsync().Result;
                var r = JsonConvert.DeserializeObject<IpVerificationResponse>(answer);
                return r;
            }

            return new IpVerificationResponse();
        }

        public string GetCountryFromIp(string ip)
        {
            var ipInfo = GetIpVerification(ip);
            if(ipInfo != null)
            {
                if (ipInfo.IsValid)
                {
                    if (ipInfo.ipdata.country == "United States") return "US";
                    if (ipInfo.ipdata.country == "United Kingdom") return "UK";
                    if (ipInfo.ipdata.country == "Canada") return "CA";
                    if (ipInfo.ipdata.country == "Australia") return "AU";
                }
            }
            return string.Empty;
        }
    }
}
