using projAPI.Model;
using projContext;
using projContext.DB;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;

namespace projAPI
{

    //This Class Fetch the date from 
    
    #region--Leave Credit and Debit logic
    

    public class EmpLeaveBalence
    {
        public EmpLeaveBalence()
        {
            EmployeeId = 0;
            TotalPreviousCredit = 0;
            TotalCredit = 0; TotalDebit = 0;
            TotalClub = 0; TotalCarryforward = 0;
            TotalExpired = 0;
        }
        public int EmployeeId { get; set; }
        public double TotalPreviousCredit { get; set; }
        public double TotalCredit { get; set; }
        public double TotalDebit { get; set; }
        public double TotalClub { get; set; }
        public double TotalCarryforward { get; set; }
        public double TotalExpired { get; set; }
    }

    #endregion


    // Loan Calculator
    public class clsLoanCalculator
    {
        // Loan EMI Calcutator
        public static double LoanEMICalculator(float loan_amount, float loan_tenure_in_months, float interest_rate)
        {
            float monthly_emi;

            interest_rate = interest_rate / 12.0f / 100.0f;

            monthly_emi = (loan_amount * interest_rate * (float)Math.Pow(1 + interest_rate, loan_tenure_in_months)) / (float)(Math.Pow(1 + interest_rate, loan_tenure_in_months) - 1);

            return monthly_emi;
        }

    }
}
