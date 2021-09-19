using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AffirmChallenge.Models
{
    public class Bank
    {
        public int BankID { get; set; }
        public string BankName { get; set; }
    }

    public sealed class BankMap : ClassMap<Bank>
    {
        public BankMap()
        {
            Map(m => m.BankID).Name("id", "BankID");
            Map(m => m.BankName).Name("name", "BankName");
        }
    }
}
