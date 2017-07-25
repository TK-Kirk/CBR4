using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBR.Core.Entities.Models
{
    public partial class CbrLead
    {
        [NotMapped]
        public DateTime? BirthDate
        {
            get
            {
                if (BirthdayYear.HasValue && BirthdayYear.Value > 0 &&
                    BirthdayMonth.HasValue && BirthdayMonth.Value > 0 &&
                    BirthdayDay.HasValue && BirthdayDay.Value > 0)
                {
                    DateTime d = new DateTime((int) BirthdayYear, (int) BirthdayMonth, (int) BirthdayDay);
                    return d;
                }
                return null;
            }

        }

    }
}
