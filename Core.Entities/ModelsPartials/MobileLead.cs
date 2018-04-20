using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBR.Core.Entities.Models
{
    public partial class MobilelLead
    {
        public bool IsRegistered
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Firstname) || string.IsNullOrWhiteSpace(Lastname))
                {
                    return false;
                }

                return true;
            }
        }
    }
}
