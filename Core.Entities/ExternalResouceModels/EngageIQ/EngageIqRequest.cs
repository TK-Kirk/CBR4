using CBR.Core.Entities.Enums;

namespace CBR.Core.Entities.ExternalResouceModels.EngageIQ
{
    public class EngageIqRequest : CoregPostRequestBase
    {
        public string Q1 { get; set; }
        public string Q2 { get; set; }
        public string Q3 { get; set; }
        public string Q4 { get; set; }
        public string Q5 { get; set; }
        public string Q6 { get; set; }

        public string Comments1 { get; set; }

    }

    public class EngageIqUpdateRequest
    {
        public EngageIqRequest RetryRequest { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Zip { get; set; }
    }

}
