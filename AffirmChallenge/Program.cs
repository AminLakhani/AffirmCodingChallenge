using System;
using AffirmChallenge.Models;
using System.Linq;
using System.Collections.Generic;
using AffirmChallenge.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace AffirmChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            var collection = new ServiceCollection();
            collection.AddScoped<ILoanService, LoanService>();

            var service = collection.BuildServiceProvider();
            var loanService = service.GetRequiredService<ILoanService>();

            Console.WriteLine("Starting Loan Processing");
            var allLoans = loanService.GetAll();
            var assignmentsAndYields = loanService.AssignLoans(allLoans);
            loanService.Publish(assignmentsAndYields.Assignments, assignmentsAndYields.Yields);
            

        }
    }
}
