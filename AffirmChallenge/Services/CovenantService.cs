using AffirmChallenge.Models;
using AffirmChallenge.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AffirmChallenge.Services
{
    public interface ICovenantService
    {
        List<Covenant> GetAll();
    }

    public class CovenantService : ICovenantService
    {
        private readonly ICovenantRepository _covenantRepo;
        private readonly IFacilityRepository _facilityRepository;

        public CovenantService(ICovenantRepository covRepo = null, IFacilityRepository facRepo = null)
        {
            _covenantRepo = covRepo ?? new CovenantRepository();
            _facilityRepository = facRepo ?? new FacilityRepository();
        }

        /// <summary>
        /// Get All Covenants including Bank level covenants
        /// </summary>
        /// <returns>List of Covenant objects</returns>
        public List<Covenant> GetAll()
        {
            var allCovenants = _covenantRepo.GetAll();

            allCovenants = AddBankLevelCovenants(allCovenants);

            //I can remove the covenants where there's no facility since I've added the one for each facility in the bank above.
            return allCovenants.Where(s => s.FacilityID != null).ToList();
        }

        private List<Covenant> AddBankLevelCovenants(List<Covenant> cov) 
        {
            var allFacilities = _facilityRepository.GetAll();

            //These are covenants that need to apply to every facility in the bank.
            foreach (Covenant c in cov.Where(s => s.FacilityID == null))
            {
                //Now I've got a list of all facilities in that bank
                foreach (Facility f in allFacilities.Where(s => s.BankID == c.BankID))
                {
                    //adding new covenants for each facility in the bank
                    cov.Add(new Covenant()
                    {
                        BankID = c.BankID,
                        FacilityID = f.FacilityID,
                        BannedState = c.BannedState,
                        MaxDefaultLikelihood = c.MaxDefaultLikelihood
                    });
                }
            }

            return cov;


        }
    }
}
