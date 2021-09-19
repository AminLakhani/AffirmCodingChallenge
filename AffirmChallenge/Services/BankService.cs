using AffirmChallenge.Models;
using AffirmChallenge.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace AffirmChallenge.Services
{
    public interface IBankService
    {
        List<Bank> GetAll();
    }

    public class BankService : IBankService
    {
        private readonly IBankRepository _bankRepo;

        public BankService(IBankRepository bankRepo = null)
        {
            _bankRepo = bankRepo ?? new BankRepository();
        }

        /// <summary>
        /// Get All Bank Items
        /// </summary>
        /// <returns>List of Bank models</returns>
        public List<Bank> GetAll()
        {
            return _bankRepo.GetAll();
        }
    }
}
