using CBR.Core.Entities.Models;
using CBR.Core.Entities.ResourceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBR.Core.Entities.ExternalResouceModels.Listrak;

namespace CBR.DataAccess.Repositories
{
    public class ListrakRepository
   {
        #region Defs    

        const string Clik_US_URL = @"http://t.lt02.net/q/FkQJw5JD15INCdv5ocCvX1l4rdwGKYAx2g";
        const string Clik_US_KEY = @"?crvs=rY-Ep4uyOGh79IOvW1LkkUkn3VIAmODbeRNOCMYZSHN0-AhVfjWO6Llxnn-VO1sF";

        const string CBR_US_Certified_URL = @"http://email.cashbackresearch.com/q/CPjQZZWQAfjkqJD9rfMsiRkKoFZrKD0XN-";

        const string CBR_US_Certified_KEY = @"?crvs=mOz6tIcv8qme4BwW0rO6SZIGDN4c141V_WweByNXIjomaXK0SUIhOJahfJ41h57UDjmIxn6QXQcLTNmzipbB6g";

        const string CBR_US_Non_Cert_URL = @"http://email.cashbackresearch.com/q/0Wjm205w2Laprg9Xqz_NYG4DEf2FK8E0LO";

        const string CBR_US_Non_Cert_KEY = @"?crvs=O5BqJ3hbdKj-rTQYwAEQJ24WanowIrB7jwlTIGQ123Y20WknSYah0j738i-s9ZR71XszIWqni0oXHcTPsHkAMA";

        const string CBR_AU_Certified_URL = @"http://email.cashbackresearch.com/q/vZ2EhPqckvFew0kPCtgg9nd7YShtKcQZVa";

        const string CBR_AU_Certified_KEY = @"?crvs=9IurZS-k_doUgDJ0gQyOqpwjmggPZH4tmsDzCxrBdYfogPhgEEVb4YDFYNpkTDmIsbybNJc_FdRPwy8h0TyEqQ";

        const string CBR_AU_Non_Cert_URL = @"http://email.cashbackresearch.com/q/R1Kj2iHm7x6OXpTfWsDLhwK7fS2qKMfvqg";

        const string CBR_AU_Non_Cert_KEY = @"?crvs=LRRZH6cBCdZ7YXReSSgpx_IlDDWMAYSyT-buQbL1mfkOSCQkG6Yffvt04mLKWu76TkE8vSdCDONOhnAptxDrVg";

        const string CBR_CA_Certified_URL = @"http://email.cashbackresearch.com/q/DO9Bi82jLKIaNWuC7evt_c9xAoi3KqOyR-";

        const string CBR_CA_Certified_KEY = @"?crvs=QgC5pWzkfmAodmF9kg-1zDC-swdNzd3Y2mo0VdopTsJb5J86gpVlZw0-INi0JjbpuVOcxrkXGiECNYFeqMpjkA";

        const string CBR_CA_Non_Cert_URL = @"http://email.cashbackresearch.com/q/9xIWXUqeiE9ebGxYsPG1j6rVm2XuK8uxCn";

        const string CBR_CA_Non_Cert_KEY = @"?crvs=i_c6XCUM2zpUdl9z7wSaUp26yyat96XyTpNRXYHUTIaiPKoZGWWkg3biIgx5S3nkpGbQm86T3vXD6eWr_6mKgw";

        const string CBR_UK_Certified_URL = @"http://email.cashbackresearch.com/q/2XTxtT9EH0nGrrt8mrZ-S5pAdytjKHuVB0";

        const string CBR_UK_Certified_KEY = @"?crvs=LRRZH6cBCdZ7YXReSSgpx_IlDDWMAYSyT-buQbL1mfkOSCQkG6Yffvt04mLKWu76TkE8vSdCDONOhnAptxDrVg";

        const string CBR_UK_Non_Cert_URL = @"http://email.cashbackresearch.com/q/SnKpC5OR57rKGbnLzbxT2ldWcdC2K80nsV";

        const string CBR_UK_Non_Cert_KEY = @"?crvs=oi4zU2Qu0l1lxWB9p6lopOnTy_rwJwy2gJVIDl9yDu5en8hfrWiIbWnnv4-MPaK4T1nTvlo4zvPU3x8zxVWG5Q";
        #endregion


        public static class Fields
        {
            public static string Email = "CBR.EmailAddress";
            public static string Firstname = "CBR.FirstName";
            public static string Lastname = "CBR.LastName";
            public static string Address = "CBR.Address";
            public static string Address2 = "CBR.Address2";
            public static string City = "CBR.City";
            public static string State = "CBR.State";
            public static string PostalCode = "CBR.PostalCode";
            public static string DayOfBirth = "CBR.DayOfBirth";
            public static string YearOfBirth = "CBR.YearOfBirth";
            public static string MonthOfBirth = "CBR.MonthOfBirth";
            public static string Gender = "CBR.Gender";
            public static string Diabetic = "CBR.Diabetic";
            public static string Glasses = "CBR.Glasses";
            public static string SmartPhone = "CBR.smartphone";
            public static string Clik = "CBR.Clik";
            public static string Device = "CBR.device";
        }
        public class ListInfo
        {
            public string Key;
            public string Url;
            public string Name;
        }




        #region private methods

        private string BuildPostData(List<FieldItem> dataItems)
        {
            string returnVal = string.Empty;
            foreach (FieldItem item in dataItems)
            {
                if (item.Name == Fields.Email)
                {
                    returnVal += "&email=" + item.Value;
                }
                returnVal += string.Format("&{0}={1}", item.Name, item.Value);
            }
            return returnVal;
        }

        private bool EnsureEmailProvided(List<FieldItem> dataItems)
        {
            //make sure at least one item is an email
            if (dataItems.All(x => x.Name != Fields.Email))
            {
                throw new ArgumentException("Email is required when updating Listrak.");
            }
            //make sure the email is not blank
            if (string.IsNullOrWhiteSpace(dataItems.FirstOrDefault(x => x.Name == Fields.Email).Value))
            {
                throw new ArgumentException("Blank email is not allowable when updating Listrak");
            }
            return true;
        }

        private ListInfo ListnameCert(string countryID)
        {
            switch (countryID)
            {
                case "US":
                    return GetKeyandUrlForList(Lists.CBR_US_Certified);
                case "UK":
                    return GetKeyandUrlForList(Lists.CBR_UK_Certified);
                case "CA":
                    return GetKeyandUrlForList(Lists.CBR_CA_Certified);
                case "AU":
                    return GetKeyandUrlForList(Lists.CBR_AU_Certified);
                default:
                    return GetKeyandUrlForList(Lists.CBR_US_Certified);
            }
        }
        private ListInfo ListnameNonCert(string countryID)
        {
            switch (countryID)
            {
                case "US":
                    return GetKeyandUrlForList(Lists.CBR_US_Non_Cert);
                case "UK":
                    return GetKeyandUrlForList(Lists.CBR_UK_Non_Cert);
                case "CA":
                    return GetKeyandUrlForList(Lists.CBR_CA_Non_Cert);
                case "AU":
                    return GetKeyandUrlForList(Lists.CBR_AU_Non_Cert);
                default:
                    return GetKeyandUrlForList(Lists.CBR_US_Non_Cert);
            }
        }

        private ListInfo GetKeyandUrlForList(Lists listname)
        {
            ListInfo li = new ListInfo();
            li.Name = listname.ToString();

            switch (listname)
            {
                case Lists.Clik_US:
                    li.Key = Clik_US_KEY;
                    li.Url = Clik_US_URL;
                    break;
                case Lists.CBR_US_Certified:
                    li.Key = CBR_US_Certified_KEY;
                    li.Url = CBR_US_Certified_URL;
                    break;
                case Lists.CBR_US_Non_Cert:
                    li.Key = CBR_US_Non_Cert_KEY;
                    li.Url = CBR_US_Non_Cert_URL;
                    break;
                case Lists.CBR_CA_Certified:
                    li.Key = CBR_CA_Certified_KEY;
                    li.Url = CBR_CA_Certified_URL;
                    break;
                case Lists.CBR_CA_Non_Cert:
                    li.Key = CBR_CA_Non_Cert_KEY;
                    li.Url = CBR_CA_Non_Cert_URL;
                    break;
                case Lists.CBR_AU_Certified:
                    li.Key = CBR_AU_Certified_KEY;
                    li.Url = CBR_AU_Certified_URL;
                    break;
                case Lists.CBR_AU_Non_Cert:
                    li.Key = CBR_AU_Non_Cert_KEY;
                    li.Url = CBR_AU_Non_Cert_URL;
                    break;
                case Lists.CBR_UK_Certified:
                    li.Key = CBR_UK_Certified_KEY;
                    li.Url = CBR_UK_Certified_URL;
                    break;
                case Lists.CBR_UK_Non_Cert:
                    li.Key = CBR_UK_Non_Cert_KEY;
                    li.Url = CBR_UK_Non_Cert_URL;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("listname");
            }
            return li;
        }
        private void GetKeyandUrlForList(Lists listname, out string key, out string url)
        {
            switch (listname)
            {
                case Lists.Clik_US:
                    key = Clik_US_KEY;
                    url = Clik_US_URL;
                    break;
                case Lists.CBR_US_Certified:
                    key = CBR_US_Certified_KEY;
                    url = CBR_US_Certified_URL;
                    break;
                case Lists.CBR_US_Non_Cert:
                    key = CBR_US_Non_Cert_KEY;
                    url = CBR_US_Non_Cert_URL;
                    break;
                case Lists.CBR_CA_Certified:
                    key = CBR_CA_Certified_KEY;
                    url = CBR_CA_Certified_URL;
                    break;
                case Lists.CBR_CA_Non_Cert:
                    key = CBR_CA_Non_Cert_KEY;
                    url = CBR_CA_Non_Cert_URL;
                    break;
                case Lists.CBR_AU_Certified:
                    key = CBR_AU_Certified_KEY;
                    url = CBR_AU_Certified_URL;
                    break;
                case Lists.CBR_AU_Non_Cert:
                    key = CBR_AU_Non_Cert_KEY;
                    url = CBR_AU_Non_Cert_URL;
                    break;
                case Lists.CBR_UK_Certified:
                    key = CBR_UK_Certified_KEY;
                    url = CBR_UK_Certified_URL;
                    break;
                case Lists.CBR_UK_Non_Cert:
                    key = CBR_UK_Non_Cert_KEY;
                    url = CBR_UK_Non_Cert_URL;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("listname");
            }
        }

        private string GetCBRFieldGroupPostData(Lead data)
        {
            //this one uses the Key info
            string postData = string.Empty;

            if (!string.IsNullOrWhiteSpace(data.Email))
            {
                postData += "&email=" + data.Email;
            }
            if (!string.IsNullOrWhiteSpace(data.Firstname))
            {
                postData += "&CBR.FirstName=" + data.Firstname;
            }
            if (!string.IsNullOrWhiteSpace(data.Lastname))
            {
                postData += "&CBR.LastName=" + data.Lastname;
            }
            if (!string.IsNullOrWhiteSpace(data.Zip))
            {
                postData += "&CBR.PostalCode=" + data.Zip;
            }

            if (data.BirthDate.HasValue)
            {
                postData += "&CBR.DayOfBirth=" + data.BirthDate.Value.Day.ToString();
                postData += "&CBR.YearOfBirth=" + data.BirthDate.Value.Year.ToString();
                postData += "&CBR.MonthOfBirth=" + data.BirthDate.Value.Month.ToString();
            }
            if (!string.IsNullOrWhiteSpace(data.Gender))
            {
                postData += "&CBR.Gender=" + data.Gender;
            }
            if (!string.IsNullOrWhiteSpace(data.Ip))
            {
                postData += "&CBR.IP=" + data.Ip;
            }
            //if any key data is populated then populate all 3
            if (!(string.IsNullOrWhiteSpace(data.OfferId) && string.IsNullOrWhiteSpace(data.SubId) &&
                  string.IsNullOrWhiteSpace(data.AffiliateId)))
            {
                postData += "&CBR.OfferID=" + data.OfferId;
                postData += "&CBR.SubID=" + data.SubId ;
                postData += "&CBR.AffiliateID=" + data.AffiliateId;
            }

            //if (data.VerificationId != null)
            //{
            //    postData += "&CBR.verificationid=" + data.VerificationId;
            //}


            //if (data.Clik.HasValue)
            //{
            //    postData += "&CBR.Clik=" + (data.Clik.Value ? "1" : "0");
            //}


            // 8/3/2013 added 2 new vars for all lists
            if (data.OfferId == "58001")
            {
                postData += "&CBR.site=Cashback Moms";
                postData += "&CBR.url=http://www.cashbackmoms.com";
            }
            else if (data.OfferId == "57044")
            {
                postData += "&CBR.site=Cashback Clik";
                postData += "&CBR.url=http://www.cashbackclik.com";
            }
            else
            {
                postData += "&CBR.site=Cashback Research";
                postData += "&CBR.url=http://www.cashbackresearch.com";
            }


            return postData;
        }


        private void AddToPostQueue(Lists listname, string postqueueName, string emailAddress, string postdata)
        {
            string key = null;
            string url = null;
            GetKeyandUrlForList(listname, out key, out url);
            AddToPostQueue(postqueueName, emailAddress, key, url, postdata);
        }
        private void AddToPostQueue(string listname, string emailAddress, string key, string url, string postData)
        {

            PostQueue pq = new PostQueue();
            if (listname?.Length > 50)
            { pq.Name = listname?.Substring(0, 50); }
            else
            { pq.Name = listname; }

            pq.Method = "GET";
            pq.EmailAddress = emailAddress;
            pq.PostUrl = url;
            pq.PostData = key + postData;
            pq.OkValue = "Thank You";
            pq.IgnoreValues = "";
            GloshareContext db = new GloshareContext();
            try
            {
                db.PostQueues.Add(pq);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                //EmailUtility.SendErrorEmail(e, "CBR", "ListtrakUtility", "UpdateListrak()");
            }
        }

        #endregion

        #region Just Called From Registration

        public bool Update_Clik_US(Lead data, string pagename, string extraData)
        {
            if (data == null) return false;

            //make sure that if the data has clik on it then update the list
            string postData = GetCBRFieldGroupPostData(data);
            //if (data.Clik.HasValue && (bool)data.Clik)
            //{
            //    postData += "&CBR.Clik=1";
            //}

            //the extra data when being called from the join button 
            //on registration.aspx is "&whitelist=1"
            if (!string.IsNullOrWhiteSpace(extraData))
            {
                postData += extraData;
            }

            AddToPostQueue(string.Format("UpdatingLT_Clik_US_{0}", pagename), data.Email, Clik_US_KEY, Clik_US_URL,
                           postData);

            return true;
        }

        public bool Update_CBR_US_Certified(Lead data, string pagename)
        {
            if (data == null) return false;

            string postData = GetCBRFieldGroupPostData(data);
            AddToPostQueue(string.Format("UpdatingLT_CBR_US_Certified_{0}", pagename), data.Email,
                           CBR_US_Certified_KEY, CBR_US_Certified_URL, postData);

            return true;
        }

        public bool Update_CBR_US_NonCert(Lead data, string pagename)
        {
            if (data == null) return false;

            string postData = GetCBRFieldGroupPostData(data);

            AddToPostQueue(string.Format("UpdatingLT_CBR_US_NonCert_{0}", pagename), data.Email, CBR_US_Non_Cert_KEY,
                           CBR_US_Non_Cert_URL, postData);

            return true;
        }

        #endregion


        public bool UpdateLists(Lead data, List<Lists> listsToUpdate, string pagename)
        {
            foreach (var list in listsToUpdate)
            {
                switch (list)
                {
                    case Lists.CBR_US_Certified:
                        Update_CBR_US_Certified(data, pagename);
                        break;
                    case Lists.CBR_US_Non_Cert:
                        Update_CBR_US_NonCert(data, pagename);
                        break;
                }
            }

            return true;
        }
        public bool UpdateLists(List<FieldItem> dataItems, List<Lists> listsToUpdate, string pagename)
        {
            //use this when you have just an email and a couple of fields to update.
            //pass the fields in th FieldItem List.
            foreach (Lists listname in listsToUpdate)
            {
                try
                {
                    if (EnsureEmailProvided(dataItems))
                    {
                        string postdata = BuildPostData(dataItems);
                        AddToPostQueue(listname, $"Adhoc_{listname}_{pagename}", dataItems.FirstOrDefault(x => x.Name == Fields.Email).Value, postdata);
                    }
                }
                catch (Exception e)
                {
                    //EmailUtility.SendErrorEmail(e, "CBR", "ListtrakUtility", "UpdateListrak()");
                    return false;
                }

            }
            return true;
        }
        public bool UpdateCert(Lead data, string pagename)
        {
            if (VerifyInput(data))
            {
                ListInfo li = ListnameCert(data.CountryId);
                string postData = GetCBRFieldGroupPostData(data);
                AddToPostQueue(string.Format("UpdatingLT_{0}_{1}", li.Name, pagename), data.Email, li.Key, li.Url, postData);
                return true;
            }
            return false;
        }

        public bool UpdateNonCert(Lead data, string pagename)
        {
            if (VerifyInput(data))
            {
                ListInfo li = ListnameNonCert(data.CountryId);
                string postData = GetCBRFieldGroupPostData(data);
                AddToPostQueue(string.Format("UpdatingLT_{0}_{1}", li.Name, pagename), data.Email, li.Key, li.Url, postData);
                return true;
            }
            return false;
        }

        private bool VerifyInput(Lead data)
        {
            if (data == null)
            {
               // EmailUtility.SendErrorEmail("Data is null in VerifyInput-LT Utility",
               //                             "vwOptInLeadUser Data is null in VerifyInput-LT Utility", true);
                return false;
            }
            if (string.IsNullOrWhiteSpace(data.CountryId))
            {
              //  EmailUtility.SendErrorEmail("Country is null in VerifyInput-LT Utility",
              //                              "vwOptInLeadUser Country is null in VerifyInput-LT Utility", true);
                return false;
            }
            return true;
        }

    }
}
