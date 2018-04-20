namespace SurveyRouter.Logic.Data.Models.YourSurveys
{
    public class Result
    {
        public string status { get; set; }
        public string user_account_status { get; set; }
        public Survey[] surveys { get; set; }
        public string[] messages { get; set; }
    }

    public class Survey
    {
        public decimal cpi { get; set; }
        public decimal conversion_rate { get; set; }
        public int loi { get; set; }
        public string name { get; set; }
        public int remaining_completes { get; set; }
        public int study_type { get; set; }
        public int project_id { get; set; }
        public string entry_link { get; set; }
    }
}
