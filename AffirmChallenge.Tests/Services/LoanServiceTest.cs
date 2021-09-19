using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AffirmChallenge.Models;
using AffirmChallenge.Services;
using AffirmChallenge.Tests.Repositories;

namespace AffirmChallenge.Tests.Services
{
    [TestClass]
    public class LoanServiceTest
    {

        [TestMethod]
        public void AssignLoans_CorrectAssignment_ReturnTrue()
        {
            ILoanService loanService = new LoanService(new LoanRepositoryTest(), new FacilityRepositoryTest(), new CovenantRepositoryTest(), new ConfigRepositoryTest());

            var allLoans = loanService.GetAll();
            var assignmentsAndYields = loanService.AssignLoans(allLoans);

            foreach (var a in assignmentsAndYields.Assignments)
            {
                if (a.LoanID == 1)
                {
                    Assert.IsTrue(a.FacilityID == 1);
                }
                else if (a.LoanID == 2)
                {
                    Assert.IsTrue(a.FacilityID == 2);
                }
                else if (a.LoanID == 3)
                {
                    Assert.IsTrue(a.FacilityID == 1);
                }
            }

        }

        [TestMethod]
        public void AssignLoans_CorrectYields_ReturnTrue()
        {
            ILoanService loanService = new LoanService(new LoanRepositoryTest(), new FacilityRepositoryTest(), new CovenantRepositoryTest(), new ConfigRepositoryTest());

            var allLoans = loanService.GetAll();
            var assignmentsAndYields = loanService.AssignLoans(allLoans);

            foreach (var y in assignmentsAndYields.Yields)
            {
                if (y.FacilityID == 1)
                {
                    Assert.IsTrue(y.ExpectedYield == 16375);
                }
                else if (y.FacilityID == 2)
                {
                    Assert.IsTrue(y.ExpectedYield == 3504);
                }
            }

        }
    }
}
