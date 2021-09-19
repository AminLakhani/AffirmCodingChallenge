using System;
using System.Collections.Generic;
using System.Text;
using AffirmChallenge.Models;
using AffirmChallenge.Repositories;

namespace AffirmChallenge.Tests.Repositories
{
    public class FacilityRepositoryTest : IFacilityRepository
    {
        public List<Facility> GetAll()
        {
            return new List<Facility>()
            {
                new Facility
                {
                    FacilityID = 2,
                    BankID = 1,
                    AllocatedAmount = 0,
                    Amount = 61104.0,
                    InterestRate = 0.07f
                },
                new Facility
                {
                    FacilityID = 1,
                    BankID = 2,
                    AllocatedAmount = 0,
                    Amount = 126122.0,
                    InterestRate = 0.06f
                },
            };
        }
    }
}
