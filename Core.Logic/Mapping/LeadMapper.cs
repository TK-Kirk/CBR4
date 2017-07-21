using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBR.Core.Entities.Models;
using CBR.Core.Entities.ResourceModels;

namespace CBR.Core.Logic.Mapping
{
    public class LeadMapper
    {
        internal static Lead Map(CbrLead cbrlead)
        {
            return new Lead()
            {
                Address = cbrlead.Address,
                Address2 = cbrlead.Address2,
                AffiliateId = cbrlead.AffiliateId,
                BirthDate = GetBirthDate(cbrlead.BirthdayDay, cbrlead.BirthdayMonth, cbrlead.BirthdayYear)
            };
        }

        internal static void Map(Lead lead, CbrLead cbrLead)
        {
            cbrLead.Address = lead.Address;
            if (lead.BirthDate.HasValue)
            {
                cbrLead.BirthdayDay = lead.BirthDate.Value.Day;
                cbrLead.BirthdayMonth = lead.BirthDate.Value.Month;
                cbrLead.BirthdayYear = lead.BirthDate.Value.Year;
            }
            cbrLead.CountryId = lead.CountryId;
            cbrLead.EmailAddress = lead.Email;
            cbrLead.Firstname = lead.Firstname;
            cbrLead.Lastname = lead.Lastname;
            cbrLead.Gender = lead.Gender?.Substring(0, 1);
            cbrLead.Phone = lead.Phone;
            cbrLead.State = lead.State;
            cbrLead.Zip = lead.Zip;

            cbrLead.OfferId = lead.OfferId;
            cbrLead.AffiliateId = lead.AffiliateId;
            cbrLead.SubId = lead.SubId;
            
        }

        private static int? GetBirthdateDay(string birthDate)
        {
            DateTime d;
            if (DateTime.TryParse(birthDate, out d))
            {
                return d.Day;
            }
            return null;
        }
        private static int? GetBirthdateMonth(string birthDate)
        {
            DateTime d;
            if (DateTime.TryParse(birthDate, out d))
            {
                return d.Month;
            }
            return null;
        }
        private static int? GetBirthdateYear(string birthDate)
        {
            DateTime d;
            if (DateTime.TryParse(birthDate, out d))
            {
                return d.Month;
            }
            return null;
        }
        private static DateTime? GetBirthDate(int? birthdayDay, int? birthdayMonth, int? birthdayYear)
        {
            DateTime returnDate;
            if (DateTime.TryParse($"{birthdayDay}/{birthdayMonth}/{birthdayYear}", out returnDate)) {
                return returnDate;
            }
            return null;
        }
    }
}
