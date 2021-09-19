using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AffirmChallenge.Models
{
    public class Facility
    {
        public int BankID { get; set; }
        public int FacilityID { get; set; }
        public float InterestRate { get; set; }
        public double Amount { get; set; }
        public double AllocatedAmount { get; set; }

        public bool IsLoanEligible(Loan l, List<Covenant> c) 
        {
            if (l.InterestRate <= InterestRate) return false;

            if (l.Amount + AllocatedAmount > Amount) return false;

            if (c.Select(s => s.BannedState).Distinct().Contains(l.State)) return false;

            //Theoretically there should only be one max default rate per facility but we'll take the last one in the list in case there are covenants with values for that field.
            var maxDefaultRate = c.Where(s => s.MaxDefaultLikelihood != null).LastOrDefault()?.MaxDefaultLikelihood;
            if (maxDefaultRate == null || maxDefaultRate < l.DefaultLikelihood) return false;

            return true;
        }

    }

    public sealed class FacilityMap : ClassMap<Facility>
    {
        public FacilityMap()
        {
            Map(m => m.BankID).Name("bank_id", "BankID");
            Map(m => m.InterestRate).Name("interest_rate", "InterestRate");
            Map(m => m.FacilityID).Name("id", "FacilityID");
            Map(m => m.Amount).Name("amount", "Amount");
        }
    }
}
