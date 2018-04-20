using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBR.Core.Entities.ResourceModels
{
    public class MobileSignupResponse
    {
        public Guid UserId { get; set; }
        public bool IsRegistered { get; set; }
        public bool ValidEmail { get; set; }
        public string Message { get; set; }

        public string DashUrl => "http://mdash.cashbackresearch.com/";
        public string RegisterUrl { get; set; }
        public string CashoutUrl => "http://m.cashbackresearch.com/cashout";
        public string EarningsUrl => "http://m.cashbackresearch.com/earnings";
        public string DailySurveysUrl { get; set; }

    }
}
