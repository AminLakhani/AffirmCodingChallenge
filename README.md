# AffirmChallenge

## How To Run

### Visual Studio

    If you've got VS you should be able to just open the project, restore nuget packages, update appSettings.json, and hit the Run button.

### Executable
    
    If you don't have VS you'll have to naviage to `\bin\Release\netcoreapp3.1`, update appSettings.json, and the run the AffirmCahllenge.exe executable. 

## Questions

1. **How long did you spend working on the problem? What did you find to be the most difficult part?**
 
      I spent ~2hrs working on the problem. There were a couple of instances where asking a few follow up questions would've helped. Like:
      * Should I round expected yield for each loan or wait till I'm adding them all up for a facility? Since on a large enough scale those fractions of a cent would add up.
      * What should the behavior be if there are multiple covenants for a facaility with a max default rate defined?

2. **How would you modify your data model or code to account for an eventual introduction of new, as-of-yet unknown types of covenants, beyond just maximum default likelihood and state restrictions?**

    The simple way would be to update the `Covenant` model with whatever additional fields you needed and then simply update the `IsLoanEligible` method in the `Facility` model to account for the new restriction. 
    
    The more complicated answer, since these new covenants are "as-of-yet unknown", is that you'd have to implement some kind of rules engine like [this one built by my current employer Microsoft.](https://github.com/microsoft/RulesEngine) That would mean covenants were driven my json configuration and you could add as many as you needed without having to touch any code, assuming you had all the data you needed.    

1. **How would you architect your solution as a production service wherein new facilities can be introduced at arbitrary points in time. Assume these facilities become available by the finance team emailing your team and describing the addition with a new set of CSVs.**

    I think the only change that would need to be made to accomodate new sets of facilities being added would be to update the `FacilityRepository` to scan a directory for CSVs rather than a single file. That would allow the finance team to drop their new set of CSVs into a directory and they would be picked up the next time the app ran.

1. **Your solution most likely simulates the streaming process by directly calling a method in your code to process the loans inside of a for loop. What would a REST API look like for this same service? Stakeholders using the API will need, at a minimum, to be able to request a loan be assigned to a facility, and read the funding status of a loan, as well as query the capacities remaining in facilities.**

    If I was building a REST API for this service, I'd probably start with each of the models getting their own endpoint with the standard four methods GET, PUT. POST, DELETE. So for example if someone want to read the funding status of a loan they'd make the following request
    ```
    GET: [URL]/[Version]/Loan/[ID]
    ```
    and the response they got back would look something like
    ```json
      {
        "ID": 1,
        "Amount": 45654,
        "InterestRate": 0.02,
        "DefaultLikelihood": 0.1,
        "State": "IL",
        "Agreements": [
        {
          "FacilityID": 1,
          "LoanID": 1
        }
        ]
      }
    ```
    
    And then based on whether the Agreements node was populated or not they'd know if the loan has been funded. 
    

1. **How might you improve your assignment algorithm if you were permitted to assign loans in batch rather than streaming? We are not looking for code here, but pseudo code or
description of a revised algorithm appreciated.**

      Being able to batch assign loans rather than stream them would have a few consequences:
      
      1. It'd allow us start at and restart ability without having to reevaluate loans we've already assigned. Ideally this is done by adding a `bool Processed` or `int BatchID` field to the `Loan` model rather than doing an expensive look-up from Assignments for each loan. 
      2. We could evaluate/sort each batch of loans versus available facilities to maximize the delta of loan interest rate and facility interest rate which I think would help increase yield. 
      3. We'd also be able to test how to best allocate loans so that we're not leaving facilities with unused capacity. 

1. **Discuss your solution’s runtime complexity.**

    All the `GetAll()` calls for each repository are `O(n)` with n being the number of lines in each CSV with the exception of the `CovenantService` which has a nested loop to assign bank level covenants to all facilities in that bank. The complexity of those loops is `O(pq)` where the worst case (and very unlikely) scenario would be p is every covenant and q is every facility. Similarly assigning loans is `O(lq)` in scenarios where no match is found where l is every loan and q is every facility.

    
