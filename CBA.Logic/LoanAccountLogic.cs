using CBA.Core;
using CBA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Logic
{
    public class LoanAccountLogic : BaseLogic<LoanAccount, LoanAccountDAO>
    {
        public const int BANKCODE = 123;
        public const int SAVINGS = 0;
        public const int CURRENT = 1;
        public const int LOAN = 2;
        public int AccountType;

        private CustomerAccountLogic customerAccountLogic = new CustomerAccountLogic();

        //Generate Unique Account Number
        public long GenerateAccountNumber(string accountType)
        {
            long AccountNumber;
            //Account Number Sequence Account Number =  10 digits, 1-3 = Bank Code, 4 = Account type, 5-10=Randomly generated

            if (accountType == "savings")
            {
                AccountType = SAVINGS;
            }
            else if (accountType == "current")
            {
                AccountType = CURRENT;
            }
            else if (accountType == "loan")
            {
                AccountType = LOAN;
            }

            do
            {
                double RandomNumber = Utils.GenerateNumber(6);
                AccountNumber = Convert.ToInt64(BANKCODE.ToString() + AccountType.ToString() + RandomNumber.ToString());
            } while (Dao.CheckAccountNumber(AccountNumber)); //if account number exists , it enters loop again

            return AccountNumber;
        }

        public IList<CustomerAccount> Search(string text)
        {
            return Dao.Search(text); 
        }

        public IList<CustomerAccount> GetAllActiveCustomerAccounts()
        {
            return Dao.GetAllActiveCustomerAccounts();
        }

        public CustomerAccount GetByAccount(long number)
        {
            return Dao.GetByAccount(number);
        }
        public bool CreditCustomerAccount(CustomerAccount account, double amount)
        {
            try
            {
                account.Balance += amount;
                account.DateModified = DateTime.Now;
                //Dao.Update(account);
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public bool DebitCustomerAccount(CustomerAccount account, double amount)
        {
            if (!IsDeductable(account,amount))
                return false;
            else
            {
                try
                {
                    account.Balance -= amount;
                    account.DateModified = DateTime.Now;
                    //Dao.Update(account);
                    return true;
                }
                catch 
                {
                    return false;
                }
            }
            
        }

        public bool IsDeductable(CustomerAccount account, double amount)
        {
            //return ((account.Balance - (account.AccountType.MinimumBalance + account.AccumulatedCharge)) > amount);
            return false;
        }

        public IList<CustomerAccount> GetByAccountType(string type)
        {
            return Dao.GetByAccountType(type);
        }
        public double CalculateCOT(CustomerAccount account, double amount)
        {
            if (account.AccountType == null) //COT only applies in current AccountType
                return amount * Convert.ToDouble(100);
            //return amount * Convert.ToDouble(account.AccountType.COT / 100);
            return 0;
        }

        //public long GenerateAccountNumber(AccountType accountType, int customerID, int branchID)
        //{
        //    int institutionID = 1;
        //    StringBuilder sb = new StringBuilder();
        //    string branchCode = branchID.ToString("D2");           
        //    int numberOfCustomerAccountWithAccountType = Dao.GetNumberOfCustomerAccountWithAccountType(customerID, accountType);
        //    int a = (int)accountType.Name;
        //    sb.AppendFormat("{0}{1}{2}{3}{4}", institutionID,
        //                                branchID.ToString("D2"),
        //                                     a, 
        //                                     numberOfCustomerAccountWithAccountType, 
        //                                     customerID.ToString("D6"));
        //    return long.Parse(sb.ToString());
            
        //}

        //public IList<CustomerAccount> GetByAccountNumber(long accountNumber)
        //{
        //    return Dao.GetByAccountNumber(accountNumber);
        //}

        public IList<LoanAccount> GetByAccountName(string accountName)
        {
            return Dao.GetByAccountName(accountName);
        }
        public IList<LoanAccount> GetByAccountNumber(long accountNumber)
        {
            return Dao.GetByAccountNumber(accountNumber);
        }

        public void ProcessNewLoan(LoanAccount newLoanAccount)
        {
            try
            {
                Dao.Insert(newLoanAccount);
                customerAccountLogic.CreditCustomerAccount(newLoanAccount.LinkedAccount, newLoanAccount.PrincipalAmount);
                NHibernateHelper.Commit();
                //msg = "success";
               // return true;
            }
            catch (Exception e)
            {
                NHibernateHelper.Rollback();
                // msg = e.Message;
               // return false;
            }
            
        }
        public long GenerateAccountNumber(AccountType accountType, int customerID, int branchID)
        {
            int institutionID = 1;
            StringBuilder sb = new StringBuilder();
            string branchCode = branchID.ToString("D2");
            int numberOfCustomerAccountWithAccountType = Dao.GetNumberOfCustomerLoans(customerID, accountType);
            int a = (int)accountType.Name;
            sb.AppendFormat("{0}{1}{2}{3}{4}", institutionID,
                                        branchID.ToString("D2"),
                                             a,
                                             numberOfCustomerAccountWithAccountType,
                                             customerID.ToString("D6"));
            return long.Parse(sb.ToString());

        }


        public IList<LoanAccount> GetAllUnpaidLoans()
        {
            return Dao.GetAllUnpaidLoans();
        }
    }
}
