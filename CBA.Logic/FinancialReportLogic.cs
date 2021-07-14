using CBA.Core;
using CBA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Logic
{
    public class FinancialReportLogic : BaseLogic<Branch, BranchDAO>
    {
        private GLAccountLogic glAccountLogic = new GLAccountLogic();
        private CustomerAccountLogic customerAccountLogic = new CustomerAccountLogic();
        private LoanAccountLogic loanAccountLogic = new LoanAccountLogic();

        public IList<TrialBalanceEntry> GetTrialBalanceEntries(out double debitTotal, out double creditTotal) 
        {
            debitTotal = 0;
            creditTotal = 0;
            IList<GLAccount> allGLAccounts = glAccountLogic.GetAll();
            IList<TrialBalanceEntry> trialBalanceEntries = new List<TrialBalanceEntry>();
            foreach (var glAccount in allGLAccounts)
            {
                bool isDebitEntry = false;
                AccountCategory mainCategory = glAccount.GLCategory.MainCategory.Name;
                switch (mainCategory)
                {
                    case AccountCategory.ASSET:
                    case AccountCategory.EXPENSE:
                        isDebitEntry = true;
                        debitTotal += glAccount.Balance;
                        break;
                    default:
                        creditTotal += glAccount.Balance;
                        break;
                }
                trialBalanceEntries.Add
                (
                    new TrialBalanceEntry 
                    {
                        GLAccountName = glAccount.Name,
                        IsDebitBalance = isDebitEntry,
                        Balance = glAccount.Balance
                    }
                );                
            }
            IList<CustomerAccount> customerAccounts = customerAccountLogic.GetAllActiveCustomerAccounts();
            double totalCustomerAccountBalance = 0;
            foreach (var customerAccount in customerAccounts)
            {
                totalCustomerAccountBalance += customerAccount.Balance;
                  
            }
            trialBalanceEntries.Add
            (
                   new TrialBalanceEntry
                   {
                       GLAccountName = "Total Customer Account Balance",
                       IsDebitBalance = false,//Customer Accounts are liabilities (Credit balance)
                       Balance = totalCustomerAccountBalance
                   }
            );
            
            IList<LoanAccount> loanAccounts = loanAccountLogic.GetAllUnpaidLoans();
            double totalLoanAccountBalance = 0;
            foreach (var loanAccount in loanAccounts)
            {
                totalLoanAccountBalance += (loanAccount.PrincipalAmount - loanAccount.AmountRepaid);
               
            }
            trialBalanceEntries.Add
            (
                   new TrialBalanceEntry
                   {
                       GLAccountName = "Total Loan Account Balance",
                       IsDebitBalance = true,//Customer Accounts are liabilities (Credit balance)
                       Balance = totalLoanAccountBalance
                   }
            );
            debitTotal += totalLoanAccountBalance;
            creditTotal   += totalCustomerAccountBalance;
            return trialBalanceEntries;
        }
        public bool IsBranchNameAvailable(string branchName)
        {
            if (Dao.GetBranchByName(branchName) == null) return true;

            return false;
        }
        public IList<Branch> SearchAll(string text)
        {
            return Dao.SearchAll(text);
        }


        public Dictionary<string, double> GetProfitAndLossEntries()
        {
            Dictionary<string, double> entries = new Dictionary<string, double>();
            double interestOnLoans = GetInterestOnLoans();
            double interestOnOtherLoans = 0;
            double totalInterestIncome = interestOnLoans;
           
            double interestOnCustomerAccounts = GetInterestOnCustomerAccounts();
            double totalInterestExpense = interestOnCustomerAccounts;
            double netInterestIncome = totalInterestIncome - totalInterestExpense;

            double cotIncome = GetAllCOT();
            double otherNonInterestIncome = GetOtherNonInterestIncome(); ;
            double totalNonInterestIncome = cotIncome + otherNonInterestIncome;

            double salaries = GetAllSalaries();
            double maintenanceCosts = GetAllMaintenanceCost();
            double otherNonInterestExpense = GetOtherNonInterestExpense();
            double totalNonInterestExpense = salaries + maintenanceCosts + otherNonInterestExpense;

            double netIncome = netInterestIncome + totalNonInterestIncome - totalNonInterestExpense;

            entries.Add("InterestOnLoans", interestOnLoans);
            entries.Add("InterestOnOtherLoans", interestOnOtherLoans);
            entries.Add("TotalInterestIncome", totalInterestIncome);

            entries.Add("InterestOnCustomerAccounts", interestOnCustomerAccounts);
            entries.Add("TotalInterestExpense", totalInterestExpense);
            entries.Add("NetInterestIncome", netInterestIncome);

            entries.Add("ChargesOnDepositAccounts", cotIncome);
            entries.Add("otherNonInterestIncome", otherNonInterestIncome);
            entries.Add("TotalNonInterestIncome", totalNonInterestIncome);

            entries.Add("Salaries", salaries);
            entries.Add("MaintenanceCosts", maintenanceCosts);
            entries.Add("otherNonInterestExpense", otherNonInterestExpense);
            entries.Add("totalNonInterestExpense", totalNonInterestExpense);

            entries.Add("netIncome", netIncome);

            return entries;
        }

        private double GetOtherNonInterestIncome()
        {
            var otherIncomes = glAccountLogic.GetOtherIncome();
            double totalBalance = 0;
            foreach (var income in otherIncomes)
            {
                totalBalance += income.Balance;
            }
            return totalBalance;
        }

        private double GetOtherNonInterestExpense()
        {
            var otherExpenses = glAccountLogic.GetOtherExpenses();
            double totalBalance = 0;
            foreach (var expense in otherExpenses)
            {
                totalBalance += expense.Balance;
            }
            return totalBalance;
        }

        private double GetAllMaintenanceCost()
        {
            var allMaintenanceCostAccount = glAccountLogic.GetAllMaintenanceGLAccounts();
            double totalBalance = 0;
            foreach (var maintenanceCostAccount in allMaintenanceCostAccount)
            {
                totalBalance += maintenanceCostAccount.Balance;
            }
            return totalBalance;
        }

        private double GetAllSalaries()
        {
            var allSalaryAccounts = glAccountLogic.GetAllSalaryGLAccounts();
            double totalBalance = 0;
            foreach (var salaryAccount in allSalaryAccounts)
            {
                totalBalance += salaryAccount.Balance;
            }
            return totalBalance;
        }

        public double GetAllCOT()
        {
            var allCOTIncomeAccount = glAccountLogic.GetAllCOTIncomeGLAccounts();
            double totalBalance = 0;
            foreach (var cotIncomeAccountAccount in allCOTIncomeAccount)
            {
                totalBalance += cotIncomeAccountAccount.Balance;
            }
            return totalBalance;
        }

        public double GetInterestOnCustomerAccounts()
        {
            var allInterestExpenseAccount = glAccountLogic.GetAllInterestExpenseAccount();
            double totalBalance = 0;
            foreach (var interestExpenseAccount in allInterestExpenseAccount)
            {
                totalBalance += interestExpenseAccount.Balance;
            }
            return totalBalance;
        }

        public double GetInterestOnLoans() 
        {
            var allInterestIncomeAccount = glAccountLogic.GetAllInterestIncomeAccount();
            double totalBalance = 0;
            foreach (var interestIncomeAccount in allInterestIncomeAccount)
            {
                totalBalance += interestIncomeAccount.Balance;
            }
            return totalBalance;
        }

        public Dictionary<string, double> GetBalanceSheetEntries()
        {
            Dictionary<string, double> entries = new Dictionary<string, double>();
            double vault = GetVaultBalance();
            double cashAssets = GetCashAssetsBalance();
            double loans = GetAllUnpaidLoans();
            double fixedAssets = 0;
            double otherAssets = GetOtherAssetsBalance();
            double totalAssets = vault + cashAssets + loans + fixedAssets + otherAssets;

            //double capital = glAccountLogic.GetGLAccountByName("Capital Account").Balance;
            double capital = glAccountLogic.GetGLAccountByName("Capital Account").Balance;
            double otherCapital = GetOtherCapitalAccountsBalance();
            double netIncome = GetNetIncome();
            double totalCapitalAndNetIncome = capital + netIncome + otherCapital;

            double customerAccountBalances = GetCustomerAccountBalance();
            double otherLiablilities = GetOtherLiabilitiesBalance();
            double totalLiabilities = customerAccountBalances + otherLiablilities;

            double totalCapitalAndLiabilities = totalCapitalAndNetIncome + totalLiabilities;

            entries.Add("vault",vault);
            entries.Add("cashAssets",cashAssets);
            entries.Add("loans",loans);
            entries.Add("fixedAssets", fixedAssets);
            entries.Add("otherAssets", otherAssets);
            entries.Add("totalAssets", totalAssets);

            entries.Add("capital", capital);
            entries.Add("otherCapital", otherCapital);
            entries.Add("netIncome", netIncome);
            entries.Add("totalCapitalAndNetIncome", totalCapitalAndNetIncome);

            entries.Add("customerAccountBalances", customerAccountBalances);
            entries.Add("otherLiablilities", otherLiablilities);

            entries.Add("totalLiabilities", totalLiabilities);

            entries.Add("totalCapitalAndLiabilities", totalCapitalAndLiabilities);

            return entries;

            //throw new NotImplementedException();
        }

        private double GetOtherCapitalAccountsBalance()
        {
            var otherCapitalGLs = glAccountLogic.GetOtherCapitalAccounts();
            double totalBalance = 0;
            foreach (var capitalGL in otherCapitalGLs)
            {
                totalBalance += capitalGL.Balance;
            }
            return totalBalance;
        }

        private double GetOtherLiabilitiesBalance()
        {
            var otherLiabilities = glAccountLogic.GetOtherLiabilities();
            double totalBalance = 0;
            foreach (var liability in otherLiabilities)
            {
                totalBalance += liability.Balance;
            }
            return totalBalance;
        }
        private double GetOtherAssetsBalance()
        {
            var otherAssets = glAccountLogic.GetOtherAssets();
            double totalBalance = 0;
            foreach (var asset in otherAssets)
            {
                totalBalance += asset.Balance;
            }
            return totalBalance;
        }        
        public double GetNetIncome() 
        {
            double interestOnLoans = GetInterestOnLoans();
            double interestOnOtherLoans = 0;
            double totalInterestIncome = interestOnLoans + interestOnOtherLoans;

            double interestOnCustomerAccounts = GetInterestOnCustomerAccounts();
            double totalInterestExpense = interestOnCustomerAccounts;
            double netInterestIncome = totalInterestIncome - totalInterestExpense;

            double cotIncome = GetAllCOT();
            double otherNonInterestIncome = GetOtherNonInterestIncome(); ;
            double totalNonInterestIncome = cotIncome + otherNonInterestIncome;

            double salaries = GetAllSalaries();
            double maintenanceCosts = GetAllMaintenanceCost();
            double otherNonInterestExpense = GetOtherNonInterestExpense();
            double totalNonInterestExpense = salaries + maintenanceCosts + otherNonInterestExpense;

            double netIncome = netInterestIncome + totalNonInterestIncome - totalNonInterestExpense;
            return netIncome;
        }

        private double GetCustomerAccountBalance()
        {
            IList<CustomerAccount> customerAccounts = customerAccountLogic.GetAllActiveCustomerAccounts();
            double totalCustomerAccountBalance = 0;
            foreach (var customerAccount in customerAccounts)
            {
                totalCustomerAccountBalance += customerAccount.Balance;

            }
            return totalCustomerAccountBalance;
        }

        private double GetAllUnpaidLoans()
        {
            var loanAccounts = loanAccountLogic.GetAllUnpaidLoans();
            double totalLoanAccountBalance = 0;
            foreach (var loanAccount in loanAccounts)
            {
                totalLoanAccountBalance += (loanAccount.PrincipalAmount- loanAccount.AmountRepaid);
            }
            return totalLoanAccountBalance;
        }

        private double GetVaultBalance()
        {
            var allVault = glAccountLogic.GetAllVaults();
            double totalBalance = 0;
            foreach (var vault in allVault)
            {
                totalBalance += vault.Balance;
            }
            return totalBalance;
        }

        private double GetCashAssetsBalance()
        {
            var allTillAccounts = glAccountLogic.GetAllTills();
            double totalBalance = 0;
            foreach (var tillAccount in allTillAccounts)
            {
                totalBalance += tillAccount.Balance;
            }
            return totalBalance;
        }
    }
}
