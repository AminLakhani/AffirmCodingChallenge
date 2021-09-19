using AffirmChallenge.Models;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace AffirmChallenge.Repositories
{
    public interface ILoanRepository
    {
        List<Loan> GetAll();
    }

    public class LoanRepository : ILoanRepository
    {
        public List<Loan> GetAll()
        {
            List<Loan> loans = new List<Loan>();
            ConfigRepository config = new ConfigRepository();

            using (var reader = new StreamReader(config.GetConfigValue("LoanCSV")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<LoanMap>();
                loans = csv.GetRecords<Loan>().ToList();
            }

            return loans;
        }
    }
}
