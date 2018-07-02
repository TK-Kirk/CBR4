using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBR.Core.Entities.ExternalResouceModels.XVerify
{
    public class EmailVerificationResponse
    {
        public bool IsValid
        {
            get
            {
                if (email != null)
                {
                    return email.responsecode == 1 || email.responsecode == 3;
                }
                return false;
            }
        }
        public Email email { get; set; }
    }

    public class Email
    {
        public string syntax { get; set; }
        public string handle { get; set; }
        public string domain { get; set; }
        public string catch_all { get; set; }
        public Auto_Correct auto_correct { get; set; }
        public int error { get; set; }
        public string message { get; set; }
        public int responsecode { get; set; }
        public string address { get; set; }
        public string status { get; set; }
        public float duration { get; set; }
    }

    public class Auto_Correct
    {
        public string corrected { get; set; }
    }

}
