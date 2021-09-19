using AffirmChallenge.Models;
using AffirmChallenge.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace AffirmChallenge.Services
{
    public interface IFacilityService
    {
        List<Facility> GetAll();
    }

    public class FacilityService : IFacilityService
    {
        private readonly IFacilityRepository _repo;

        public FacilityService(IFacilityRepository faRepo = null)
        {
            _repo = faRepo ?? new FacilityRepository();
        }

        /// <summary>
        /// Returns all facilities
        /// </summary>
        /// <returns>List of facility model.</returns>
        public List<Facility> GetAll()
        {
            return _repo.GetAll();
        }
    }
}
