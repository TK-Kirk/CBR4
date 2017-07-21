using CBR.Core.Entities.ResourceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBR.Core.Entities.ExternalResouceModels.Listrak
{
    public class CoregPostRequest
    {
        public List<Lists> ListsToUpdate;
        public Lead Data { get; set; }

        public string PageName { get; set; }
    }

    public class AdHocRequest {
        public List<FieldItem> FieldItems { get; set; }
        public List<Lists> ListsToUpdate;
        public string PageName { get; set; }
    }

    public class FieldItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public enum Lists
    {
        None,
        Clik_US = 1,
        CBR_US_Certified,
        CBR_CA_Certified,
        CBR_AU_Certified,
        CBR_UK_Certified,
        CBR_US_Non_Cert,
        CBR_CA_Non_Cert,
        CBR_AU_Non_Cert,
        CBR_UK_Non_Cert
    }
}
