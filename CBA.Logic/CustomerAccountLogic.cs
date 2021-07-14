using CBA.Core;
using CBA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Logic
{
    public class CustomerAccountLogic : BaseLogic<CustomerAccount, CustomerAccountDAO>
    {
        public const int BANKCODE = 123;
        public const int SAVINGS = 0;
        public const int CURRENT = 1;
        public const int LOAN = 2;
        public int AccountType;
        private SavingsConfigDAO savingsConfigDAO = new SavingsConfigDAO();

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
            account.Balance += amount;           
            Dao.Update(account);
            //try
            //{
            //    account.Balance += amount;
            //    account.DateModified = DateTime.Now;
            //    Dao.Update(account);
            //    return true;
            //}
            //catch 
            //{
            //    return false;
            //}
            return true;
        }
        public bool ReverseCustomerAccountDebit(CustomerAccount account, double amount)
        {
            account.Balance += amount;
            if (account.AccountType.Name == AccountTypeEnum.CURRENT)
            {
                double cot = Dao.GetCurrentAccountCOT() * (amount / 1000);
                account.AccumulatedCharge -= cot;
            }
            Dao.Update(account);           
            return true;
        }

        public bool DebitCustomerAccount(CustomerAccount account, double amount)
        {
            if (!IsDeductable(account,amount)) return false;

            account.Balance -= amount;
            if (account.AccountType.Name == AccountTypeEnum.CURRENT)
            {
                double cot = Dao.GetCurrentAccountCOT() * (amount / 1000);
                account.AccumulatedCharge += cot;
            }                            
            Dao.Update(account);
            return true;            
        }

        public bool IsDeductable(CustomerAccount account, double amount)
        {
            double minimumBalance = getAccountTypeMinimumBalance(account.AccountType);
            double cot = 0; 
            if (account.AccountType.Name == AccountTypeEnum.CURRENT)           
                cot = Dao.GetCurrentAccountCOT() * (amount/1000);
            
            //return account.Balance >= amount;
            //cot will be zero if it is a savings 
            return ((account.Balance - (minimumBalance + account.AccumulatedCharge + cot)) > amount);
            //return true;
        }

        private double getAccountTypeMinimumBalance(AccountType accountType) 
        {
            if (accountType.Name == AccountTypeEnum.SAVINGS)
            {
                return Dao.GetAccountTypeMinimumBalance<SavingsAccountConfiguration>();
            }
            return Dao.GetAccountTypeMinimumBalance<CurrentAccountConfiguration>();
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

        public long GenerateAccountNumber(AccountType accountType, int customerID, int branchID)
        {
            int institutionID = 1;
            StringBuilder sb = new StringBuilder();
            string branchCode = branchID.ToString("D2");           
            int numberOfCustomerAccountWithAccountType = Dao.GetNumberOfCustomerAccountWithAccountType(customerID, accountType);
            int a = (int)accountType.Name;
            sb.AppendFormat("{0}{1}{2}{3}{4}", institutionID,
                                        branchID.ToString("D2"),
                                             a, 
                                             numberOfCustomerAccountWithAccountType, 
                                             customerID.ToString("D6"));
            return long.Parse(sb.ToString());
            
        }

        public IList<CustomerAccount> GetByAccountNumber(long accountNumber, bool includeOnlyActive = true)
        {
            return Dao.GetByAccountNumber(accountNumber,includeOnlyActive);
        }
        public CustomerAccount GetCustomerAccountByAccountNumber(long accountNumber, bool includeOnlyActive = true)
        {
            return GetByAccountNumber(accountNumber).FirstOrDefault();
            //return Dao.GetCustomerAccountByAccountNumber(accountNumber, includeOnlyActive);
        }

        public IList<CustomerAccount> GetByAccountName(string accountName, bool includeOnlyActive = true)
        {
            return Dao.GetByAccountName(accountName, includeOnlyActive);
        }
    }
}
