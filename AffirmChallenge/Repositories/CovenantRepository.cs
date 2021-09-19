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
    public interface ICovenantRepository
    {
        List<Covenant> GetAll();
    }

    public class CovenantRepository : ICovenantRepository
    {
        public List<Covenant> GetAll()
        {
            List<Covenant> covenants = new List<Covenant>();
            ConfigRepository config = new ConfigRepository();

            using (var reader = new StreamReader(config.GetConfigValue("CovenantCSV")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<CovenantMap>();
                covenants = csv.GetRecords<Covenant>().ToList();
            }

            return covenants;
        }
    }
}

