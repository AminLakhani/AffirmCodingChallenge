using AffirmChallenge.Models;
using AffirmChallenge.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using CsvHelper;
using System.Globalization;

namespace AffirmChallenge.Services
{
    public interface ILoanService
    {
        List<Loan> GetAll();
        AssignedLoansYields AssignLoans(List<Loan> loans);
        void Publish(List<Assignment> assignments, List<Yield> yields);
    }

    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepo;
        private readonly IFacilityRepository _facilityRepository;
        private readonly ICovenantRepository _covenantRepository;
        private readonly IConfigRepository _configRepository;

        public LoanService(ILoanRepository loanRepo = null, IFacilityRepository facRepo = null, ICovenantRepository covRepo = null, IConfigRepository conRepo = null)
        {
            _loanRepo = loanRepo ?? new LoanRepository();
            _facilityRepository = facRepo ?? new FacilityRepository();
            _covenantRepository = covRepo ?? new CovenantRepository();
            _configRepository = conRepo ?? new ConfigRepository();
        }

        /// <summary>
        /// Get all laons
        /// </summary>
        /// <returns>List of loan model</returns>
        public List<Loan> GetAll()
        {
            return _loanRepo.GetAll();
        }


        /// <summary>
        /// Assign Loans and outputs Yield and Assignments
        /// </summary>
        /// <param name="loans">Loans you want to process</param>
        public AssignedLoansYields AssignLoans(List<Loan> loans)
        {
            List<Assignment> assignments = new List<Assignment>();
            List<Yield> yields = new List<Yield>();

            //order this list once by interest rate then by amount to give preferability to larger facilities. Assignment doesn't say how to break ties so figured I got to keep the big boys happy.
            List<Facility> facilities = _facilityRepository.GetAll().OrderBy(o => o.InterestRate).ThenByDescending(t => t.Amount).ToList();
            List<Covenant> covenants = _covenantRepository.GetAll();

            foreach (Loan l in loans)
            {
                Facility selectedFacility = AssignFacility(l, covenants, facilities);
                if (selectedFacility != null)
                {

                    Console.WriteLine("Processing Loan " + l.ID + " - Assigning Facility " + selectedFacility.FacilityID);
                    assignments.Add(new Assignment { FacilityID = selectedFacility.FacilityID, LoanID = l.ID });
                    //Just going to raw add the yield for every loan here and can group/sum later.
                    yields.Add(new Yield
                    {
                        FacilityID = selectedFacility.FacilityID,
                        ExpectedYield = CalculateExpectedYield(
                            l.DefaultLikelihood,
                            l.InterestRate,
                            l.Amount,
                            selectedFacility.InterestRate
                            )
                    });

                    //Update how much we've allocated so far
                    facilities.First(s => s.FacilityID == selectedFacility.FacilityID).AllocatedAmount += l.Amount;

                }
                else
                {
                    //Couldn't find a facility.
                    assignments.Add(new Assignment { FacilityID = null, LoanID = l.ID });
                    Console.WriteLine("Processing Loan " + l.ID + " NO FACILITY FOUND");
                }
            }

            //Summing up total yields per facility
            List<Yield> finalYields = yields
                .GroupBy(s => s.FacilityID)
                .Select(l => new Yield { FacilityID = l.Key, ExpectedYield = Math.Round(l.Sum(c => c.ExpectedYield)) })
                .ToList();

            //If a facility doesn't get assigned anything we still might want to show it in the report.
            foreach (Facility f in facilities.Where(s => s.AllocatedAmount == 0))
            {
                finalYields.Add(new Yield { FacilityID = f.FacilityID, ExpectedYield = 0 });
            
            }

            return new AssignedLoansYields { Assignments = assignments, Yields = finalYields };

        }

        private Facility AssignFacility(Loan l, List<Covenant> covenants, List<Facility> facilities)
        {

            //If you're looking for where I handle covenants with no facility that need to apply to all facilities in a bank, it's in the covenant service.
            foreach (Facility f in facilities)
            {
                var fc = covenants.Where(s => s.FacilityID == f.FacilityID).ToList();
                if (f.IsLoanEligible(l, fc))
                {
                    return f;
                }
            }

            return null;
        }

        private double CalculateExpectedYield(float defaultRate, float loanRate, double amount, float faceilityRate)
        {
            return Math.Round((1 - defaultRate) * (loanRate * amount) - (defaultRate * amount) - (faceilityRate * amount));
        }

        public void Publish(List<Assignment> assignments, List<Yield> yields)
        {
            Console.WriteLine("Publishing CSVs");
            using (var writer = new StreamWriter(_configRepository.GetConfigValue("AssignmentCSV")))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(assignments.OrderBy(o => o.LoanID));
            }

            using (var writer = new StreamWriter(_configRepository.GetConfigValue("YieldCSV")))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(yields.OrderBy(o => o.FacilityID));
            }
        }
    }
}
