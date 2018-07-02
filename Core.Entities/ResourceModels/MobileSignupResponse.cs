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

        public string DashUrl { get; set; }

        public string RegisterUrl { get; set; }
        public string CashoutUrl => "http://m.cashbackresearch.com/cashout";
        public string EarningsUrl { get; set; }
        public string DailySurveysUrl { get; set; }

        public string DisplayName { get; set; }

    }
}
