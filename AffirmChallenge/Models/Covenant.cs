using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AffirmChallenge.Models
{
    public class Covenant
    {
        public int? FacilityID { get; set; }
        public float? MaxDefaultLikelihood { get; set; }
        public int BankID { get; set; }
        public string BannedState { get; set; }

    }

    public sealed class CovenantMap : ClassMap<Covenant>
    {
        public CovenantMap()
        {
            Map(m => m.BankID).Name("bank_id", "BankID");
            Map(m => m.FacilityID).Name("facility_id", "FacilityID");
            Map(m => m.MaxDefaultLikelihood).Name("max_default_likelihood", "MaxDefaultLikelihood");
            Map(m => m.BannedState).Name("banned_state", "BannedState");
        }
    }
}
