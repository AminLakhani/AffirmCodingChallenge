using System;
using System.Collections.Generic;
using System.Text;
using AffirmChallenge.Models;
using AffirmChallenge.Repositories;

namespace AffirmChallenge.Tests.Repositories
{
    public class LoanRepositoryTest : ILoanRepository
    {
        public List<Loan> GetAll()
        {
            return new List<Loan>()
            {
               new Loan
               {
                   Amount = 10552.0,
                   InterestRate = .15f,
                   ID = 1,
                   DefaultLikelihood = 0.02f,
                   State = "MO"
               },
                new Loan
               {
                   Amount = 51157.0,
                   InterestRate = .15f,
                   ID = 2,
                   DefaultLikelihood = 0.01f,
                   State = "VT"
               },
                new Loan
               {
                   Amount = 74965,
                   InterestRate = .35f,
                   ID = 3,
                   DefaultLikelihood = 0.06f,
                   State = "AL"
               }
            };
        }
    }
}
