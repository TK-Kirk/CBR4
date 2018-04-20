using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SurveyRouter.Logic.Data;

namespace SurveyRouter.Logic.Logic
{
    public class PostbackManager
    {
        public void PostbackPrecisionSample_OLD(string ug, string tx_id, decimal reward, string status, DateTime date, int surveyid, decimal gross)
        {
            var db = new GloshareDbContext();
            Guid userguid = Guid.Parse(ug);
            ;
            var pb = db.RouterPostBackPrecisionSamples.FirstOrDefault(p =>
                p.UserGuid == userguid && p.ProjectId == surveyid);

            if (pb == null)
            {
                pb = new RouterPostBackPrecisionSample()
                {
                    UserGuid = userguid,
                    //TransactionId = tx_id,
                    ProjectId = surveyid,
                    Gross = gross,
                    PostbackDate = date,
                    Reward = reward,
                    Status = status //should not be getting an R when created
                };
                db.RouterPostBackPrecisionSamples.Add(pb);
            }

            if (status == "R")
            {
                pb.Reversed = true;
                pb.ReverseDate = DateTime.Now;
            }

            db.SaveChanges();
        }


        public void PostbackPrecisionSample(string ug, string sub_id, decimal reward, string status, DateTime date, int surveyid, decimal gross)
        {
            var db = new GloshareDbContext();
            Guid userguid = Guid.Parse(ug);
            Guid transId = Guid.Parse(sub_id);
            //var pb = db.RouterPostBackPrecisionSamples.FirstOrDefault(p =>
            //    p.UserGuid == userguid && p.ProjectId == surveyid);
            var pb = db.RouterPostBackPrecisionSamples.FirstOrDefault(p => p.TransactionId == transId);

            if (pb == null)
            {
                pb = new RouterPostBackPrecisionSample()
                {
                    UserGuid = userguid,
                    TransactionId = transId,
                    ProjectId = surveyid,
                    Gross = gross,
                    PostbackDate = date,
                    Reward = reward,
                    Status = status //should not be getting an R when created
                };
                db.RouterPostBackPrecisionSamples.Add(pb);
            }

            if (status == "R")
            {
                pb.Reversed = true;
                pb.ReverseDate = DateTime.Now;
            }

            db.SaveChanges();
        }
        public void PostbackYourSurveys(string ssi2, string ssi3, string ip, string transactionId, string signatureMd5)
        {
           var db = new GloshareDbContext();
            var transId = Guid.Parse(ssi2);
            var pb = db.RouterPostBackYourSurveys.FirstOrDefault(p => p.TransactionId == transId);
            if (pb == null)
            {
                pb = new RouterPostBackYourSurvey()
                {
                    IpAddress = ip,
                    PostbackDate = DateTime.Now,
                    SignatureMd5 = signatureMd5,
                    TransactionId = transId,
                    TransactionIdYs = transactionId
                };
                db.RouterPostBackYourSurveys.Add(pb);
                db.SaveChanges();
            }

        }

        public bool VerifyYourSurveysHash(string supplierSubId, string transactionId, string signatureMd5)
        {
            var hashedInput =
                MD5Hash($"{supplierSubId}:{transactionId}:{ConfigurationManager.AppSettings["YourSurveysSecretKey"]}");

            return hashedInput == signatureMd5;

        }

        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
