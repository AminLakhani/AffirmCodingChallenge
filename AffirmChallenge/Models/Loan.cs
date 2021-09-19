using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AffirmChallenge.Models
{
    public class Loan
    {
        public int ID { get; set; }
        public double Amount { get; set; }
        public float InterestRate { get; set; }
        public float DefaultLikelihood { get; set; }
        public string State { get; set; }

    }

    public sealed class LoanMap : ClassMap<Loan>
    {
        public LoanMap()
        {
            Map(m => m.InterestRate).Name("interest_rate", "InterestRate");
            Map(m => m.DefaultLikelihood).Name("default_likelihood", "DefaultLikelihood");
            Map(m => m.State).Name("state", "State");
            Map(m => m.Amount).Name("amount", "Amount");
            Map(m => m.ID).Name("id", "ID");
        }
    }
}
