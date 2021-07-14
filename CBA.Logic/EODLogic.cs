using CBA.Core;
using CBA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Logic
{
    public class EODLogic : BaseLogic<EOD, EODDAO>
    {
        private GLAccountLogic glAccountLogic = new GLAccountLogic();
        private GLAccountDAO glAccountDAO = new GLAccountDAO();
        private CustomerAccountLogic customerAccountLogic = new CustomerAccountLogic();
        private CustomerAccountDAO customerAccountDAO = new CustomerAccountDAO();
        private LoanAccountDAO loanAccountDAO = new LoanAccountDAO();
        private LoanAccountLogic loanAccountLogic = new LoanAccountLogic();
        private FinancialDateDAO finDateDAO = new FinancialDateDAO();
        //UserLogic userBus = new UserLogic();
        //single eod only
        public EOD GetOrSetEOD(string username)
        {
            if (GetById(1) != null)
            {
                return GetById(1);
            }
            else
            {
                EOD eod = new EOD();
                string msg = "";
                eod.BusinessStatus = Business.OPEN;
               // eod.SuperAdmin = null;//userBus.GetByUserName(username);
                eod.FinancialDate = DateTime.Now.AddDays(-1.0);
                eod.DateCreated = DateTime.Now;
                eod.DateModified = DateTime.Now;
                Insert(eod, out msg);
                return eod;
            }
        }
        public bool IsBusinessOpen()
        {
            if (new EODLogic().GetById(1).BusinessStatus != Business.OPEN)
            {
                return false;
            }
            return true;
        }

        public void RunEOD(SavingsAccountConfiguration savingsConfig, CurrentAccountConfiguration currentAccountConfig ,LoanAccountConfiguration loansConfig)
        {
            IList<CustomerAccount> customerAccounts = customerAccountLogic.GetAllActiveCustomerAccounts();
            double interest = 0;
            //double interestIncome = 0;
            double interestPayable = 0;
            double interestIncome = 0;
            //double currentnterestPayable = 0;
            double cot = 0;
            double cotIncome = 0;

            foreach (var customerAccount in customerAccounts)
            {
                if (customerAccount.AccountType.Name == AccountTypeEnum.SAVINGS)
                {
                    interest = ((savingsConfig.CreditInterestRate / 100) * customerAccount.Balance) / 365;
                    customerAccount.Balance += interest;
                    interestPayable += interest;
                }
                else
                {
                    cot = customerAccount.AccumulatedCharge;
                    customerAccount.Balance -= cot;
                    customerAccount.AccumulatedCharge = 0;
                    cotIncome += cot;
                    interest = ((currentAccountConfig.CreditInterestRate / 100) * customerAccount.Balance) / 365;
                    
                }
                
            }
            IList<LoanAccount> loanAccounts = loanAccountLogic.GetAllUnpaidLoans();
            double debitInterest = 0;
            foreach (var loanAccount in loanAccounts)
            {
                if (loanAccount.NextDueDate == new FinancialDateLogic().GetById(1).CurrentFinancialDate)
                {
                    CustomerAccount linkedAccount = loanAccount.LinkedAccount;
                    debitInterest = (((loansConfig.DebitInterestRate / 100) * loanAccount.PrincipalAmount) / 365)*loanAccount.RepaymentInterval;
                    if (linkedAccount.Balance >= debitInterest)
                    {
                        linkedAccount.Balance -= debitInterest;
                        interestIncome += debitInterest;
                    }
                    if (linkedAccount.Balance >= loanAccount.InstallmentAmount)
                    {
                        linkedAccount.Balance -= loanAccount.InstallmentAmount;
                        loanAccount.AmountRepaid += loanAccount.InstallmentAmount;
                        //interestIncome += debitInterest;
                    }
                    if (loanAccount.AmountRepaid >= loanAccount.PrincipalAmount)
                    {
                        loanAccount.Status.Name = LoanStatusEnum.PAID;
                    }
                    else
                    {
                        loanAccount.NextDueDate = new FinancialDateLogic().GetById(1).CurrentFinancialDate.AddDays(loanAccount.RepaymentInterval);
                    }
                }
            }

            GLAccount interestPayableGLAccount = glAccountLogic.GetById(savingsConfig.InterestExpenseGLAccount.ID);
            interestPayableGLAccount.Balance += interestPayable;
            GLAccount cotIncomeGLAccount = glAccountLogic.GetById(currentAccountConfig.COTIncomeGLAccount.ID);
            cotIncomeGLAccount.Balance += cotIncome;
            GLAccount interestIncomeGLAccount = glAccountLogic.GetById(loansConfig.InterestIncomeGLAccount.ID);
            interestIncomeGLAccount.Balance += interestIncome;

            try
            {
                foreach (var customerAccount in customerAccounts)
                {
                    customerAccountDAO.Update(customerAccount);
                }
                foreach (var loanAccount in loanAccounts) 
                {
                    loanAccountDAO.Update(loanAccount);
                }
                glAccountDAO.Update(interestPayableGLAccount);
                glAccountDAO.Update(cotIncomeGLAccount);
                glAccountDAO.Update(interestIncomeGLAccount);

                FinancialDate financialDate = finDateDAO.GetById(1); 

                EOD eod = Dao.GetById(1);
                eod.BusinessStatus = Business.CLOSED;
                eod.FinancialDate = financialDate.CurrentFinancialDate.AddDays(1);
                Dao.Update(eod);
               // FinancialDate financialDate = finDateDAO.GetById(1);
                financialDate.CurrentFinancialDate =financialDate.CurrentFinancialDate.AddDays(1);
                finDateDAO.Update(financialDate);
                NHibernateHelper.Commit();
            }
            catch (Exception)
            {
                NHibernateHelper.Rollback();                
            }
            
        }

        public void OpenBusiness()
        {
            EOD eod = Dao.GetById(1);
            eod.BusinessStatus = Business.OPEN;
            Update(eod);
        }
    }
}
