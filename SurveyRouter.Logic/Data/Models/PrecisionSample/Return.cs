using System;

namespace SurveyRouter.Logic.Data.Models.PrecisionSample
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.

    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://tempuri.org/", IsNullable = false)]
    public partial class Wrapper
    {
        public Surveys Surveys { get; set; }
    }

    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Surveys
    {

        private SurveysSurvey[] surveyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Survey")]
        public SurveysSurvey[] Survey
        {
            get
            {
                return this.surveyField;
            }
            set
            {
                this.surveyField = value;
            }
        }
    }

    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SurveysSurvey
    {

        private int projectIdField;

        private string surveyNameField;

        private byte surveyLengthField;

        private decimal grossRevenueField;

        private decimal rewardValueField;

        private byte rewardPointsField;

        private string surveyTrafficTypeField;

        private string surveyUrlField;

        private byte irField;

        private string verityCheckRequiredField;

        /// <remarks/>
        public int ProjectId
        {
            get
            {
                return this.projectIdField;
            }
            set
            {
                this.projectIdField = value;
            }
        }

        /// <remarks/>
        public string SurveyName
        {
            get
            {
                return this.surveyNameField;
            }
            set
            {
                this.surveyNameField = value;
            }
        }

        /// <remarks/>
        public byte SurveyLength
        {
            get
            {
                return this.surveyLengthField;
            }
            set
            {
                this.surveyLengthField = value;
            }
        }

        /// <remarks/>
        public decimal GrossRevenue
        {
            get
            {
                return this.grossRevenueField;
            }
            set
            {
                this.grossRevenueField = value;
            }
        }

        /// <remarks/>
        public decimal RewardValue
        {
            get
            {
                return this.rewardValueField;
            }
            set
            {
                this.rewardValueField = value;
            }
        }

        /// <remarks/>
        public byte RewardPoints
        {
            get
            {
                return this.rewardPointsField;
            }
            set
            {
                this.rewardPointsField = value;
            }
        }

        /// <remarks/>
        public string SurveyTrafficType
        {
            get
            {
                return this.surveyTrafficTypeField;
            }
            set
            {
                this.surveyTrafficTypeField = value;
            }
        }

        /// <remarks/>
        public string SurveyUrl
        {
            get
            {
                return this.surveyUrlField;
            }
            set
            {
                this.surveyUrlField = value;
            }
        }

        /// <remarks/>
        public byte IR
        {
            get
            {
                return this.irField;
            }
            set
            {
                this.irField = value;
            }
        }

        /// <remarks/>
        public string VerityCheckRequired
        {
            get
            {
                return this.verityCheckRequiredField;
            }
            set
            {
                this.verityCheckRequiredField = value;
            }
        }
    }


    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class result
    {

        private string messageField;

        private string userGuidField;

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        public string UserGuid
        {
            get
            {
                return this.userGuidField;
            }
            set
            {
                this.userGuidField = value;
            }
        }
    }




}
