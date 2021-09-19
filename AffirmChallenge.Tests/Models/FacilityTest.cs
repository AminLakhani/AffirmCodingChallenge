using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AffirmChallenge.Models;

namespace AffirmChallenge.Tests.Models
{
    [TestClass]
    public class FacilityTest
    {
        [TestMethod]
        public void IsLoanEligible_StateIsBanned_ReturnFalse()
        {
            Loan l = new Loan 
            {
                ID = 1,
                Amount = 500,
                DefaultLikelihood = 0.02f,
                InterestRate = .2f,
                State = "IL"
            };

            Facility f = new Facility 
            {
                BankID = 1,
                Amount = 2000,
                AllocatedAmount = 0,
                InterestRate = .01f,
                FacilityID = 1
            };

            Covenant c = new Covenant 
            {
                BankID = 1,
                FacilityID = 1,
                BannedState = "IL",
                MaxDefaultLikelihood = .09f
            };

            Assert.IsFalse(f.IsLoanEligible(l, new List<Covenant>() { c }));

        }

        [TestMethod]
        public void IsLoanEligible_LoanExceedsMaxDefaultRateOrNull_ReturnFalse()
        {
            Loan l = new Loan
            {
                ID = 1,
                Amount = 500,
                DefaultLikelihood = 1f,
                InterestRate = .2f,
                State = "IL"
            };

            Facility f = new Facility
            {
                BankID = 1,
                Amount = 2000,
                AllocatedAmount = 0,
                InterestRate = .01f,
                FacilityID = 1
            };

            Covenant c = new Covenant
            {
                BankID = 1,
                FacilityID = 1,
                BannedState = "CA",
                MaxDefaultLikelihood = .09f
            };

            Assert.IsFalse(f.IsLoanEligible(l, new List<Covenant>() { c }));

            c.MaxDefaultLikelihood = null;
            Assert.IsFalse(f.IsLoanEligible(l, new List<Covenant>() { c }));
        }

        [TestMethod]
        public void IsLoanEligible_LoanExceedsCapacity_ReturnFalse()
        {
            Loan l = new Loan
            {
                ID = 1,
                Amount = 5000,
                DefaultLikelihood = 0.01f,
                InterestRate = .2f,
                State = "IL"
            };

            Facility f = new Facility
            {
                BankID = 1,
                Amount = 2000,
                AllocatedAmount = 0,
                InterestRate = .01f,
                FacilityID = 1
            };

            Covenant c = new Covenant
            {
                BankID = 1,
                FacilityID = 1,
                BannedState = "CA",
                MaxDefaultLikelihood = .09f
            };

            Assert.IsFalse(f.IsLoanEligible(l, new List<Covenant>() { c }));

        }

        [TestMethod]
        public void IsLoanEligible_LoanHasNegativeYield_ReturnFalse()
        {
            Loan l = new Loan
            {
                ID = 1,
                Amount = 500,
                DefaultLikelihood = 0.01f,
                InterestRate = .02f,
                State = "IL"
            };

            Facility f = new Facility
            {
                BankID = 1,
                Amount = 2000,
                AllocatedAmount = 0,
                InterestRate = .1f,
                FacilityID = 1
            };

            Covenant c = new Covenant
            {
                BankID = 1,
                FacilityID = 1,
                BannedState = "CA",
                MaxDefaultLikelihood = .09f
            };

            Assert.IsFalse(f.IsLoanEligible(l, new List<Covenant>() { c }));

        }

        [TestMethod]
        public void IsLoanEligible_LoanIsEligibe_ReturnTrue()
        {
            Loan l = new Loan
            {
                ID = 1,
                Amount = 500,
                DefaultLikelihood = 0.08f,
                InterestRate = .1f,
                State = "IL"
            };

            Facility f = new Facility
            {
                BankID = 1,
                Amount = 2000,
                AllocatedAmount = 0,
                InterestRate = .01f,
                FacilityID = 1
            };

            Covenant c = new Covenant
            {
                BankID = 1,
                FacilityID = 1,
                BannedState = "CA",
                MaxDefaultLikelihood = .09f
            };

            Assert.IsTrue(f.IsLoanEligible(l, new List<Covenant>() { c }));

        }

    }
}
