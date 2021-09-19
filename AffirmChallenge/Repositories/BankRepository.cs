using AffirmChallenge.Models;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace AffirmChallenge.Repositories
{
    public interface IBankRepository 
    {
        List<Bank> GetAll();
    }

    public class BankRepository : IBankRepository
    {
        public List<Bank> GetAll()
        {
            List<Bank> banks = new List<Bank>();
            ConfigRepository config = new ConfigRepository();

            using (var reader = new StreamReader(config.GetConfigValue("BankCSV")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<BankMap>();
                banks = csv.GetRecords<Bank>().ToList();
            }

            return banks;
        }

    }
}
