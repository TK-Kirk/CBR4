using CBR.Core.Entities.ResourceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBR.Core.Entities.ExternalResouceModels.ProvideMedia
{
    public class ProvideMediaRequest : CoregPostRequestBase
    {
        public string TrustedForm { get; set; }

    }

    public class ProvideMediaUpdateRequest
    {
        public ProvideMediaRequest RetryRequest { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Zip { get; set; }
    }

}
