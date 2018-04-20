using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBR.Core.Entities.ResourceModels
{
    public class UserRegistrationResponse
    {
        public bool ZipIsValid { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
