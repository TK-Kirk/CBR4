using System.Collections.Generic;

namespace SurveyRouter.Logic.Data.Models
{
    public class RouterReturnContainer
    {
        public RouterReturnContainer()
        {
            RouterReturnList = new List<RouterReturn>();
        }
        public string Message { get; set; }
        public  List<RouterReturn> RouterReturnList { get; set; }
    }
    public class RouterReturn
    {
        //public string URL { get; set; }
        //public int HostId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        //public string Name { get; set; }

        //public decimal SuccessRate { get; set; }

        public decimal EarningPotential { get; set; }

        //public int ProjectId { get; set; }

        public string ProxyUrl { get; set; }
    }
}
