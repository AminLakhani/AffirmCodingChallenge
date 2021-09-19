using System;
using System.Collections.Generic;
using System.Text;
using AffirmChallenge.Models;
using AffirmChallenge.Repositories;

namespace AffirmChallenge.Tests.Repositories
{
    public class CovenantRepositoryTest : ICovenantRepository
    {
        public List<Covenant> GetAll()
        {
            return new List<Covenant>
            {
                new Covenant()
                {
                    BankID = 1,
                    BannedState = "MT",
                    FacilityID = 2,
                    MaxDefaultLikelihood = 0.09f
                },
                new Covenant()
                {
                    BankID = 2,
                    BannedState = "VT",
                    FacilityID = 1,
                    MaxDefaultLikelihood = 0.09f
                },
                new Covenant()
                {
                    BankID = 2,
                    BannedState = "CA",
                    FacilityID = 1,
                    MaxDefaultLikelihood = null
                }
            };
        }
    }
}
