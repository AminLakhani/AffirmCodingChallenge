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
    public interface IFacilityRepository
    {
        List<Facility> GetAll();
    }

    public class FacilityRepository : IFacilityRepository
    {
        public List<Facility> GetAll()
        {
            List<Facility> facilities = new List<Facility>();
            ConfigRepository config = new ConfigRepository();

            using (var reader = new StreamReader(config.GetConfigValue("FacilityCSV")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<FacilityMap>();
                facilities = csv.GetRecords<Facility>().ToList();
            }

            return facilities;
        }
    }
}
