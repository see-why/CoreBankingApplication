using CBA.Core;
using CBA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Logic
{
    public class CardTransactionLogic : BaseLogic<CardTransaction, CardTransactionDAO>
    {
        GLAccountLogic glAccountLogic = new GLAccountLogic();
        CustomerAccountLogic customerAccountLogic = new CustomerAccountLogic();
        public bool ProcessCardWithDraw(CustomerAccount customerAccount, double amount, double charge)
        {
            try
            {
                if (!glAccountLogic.CreditATMTillAccount(amount))
                    return false;
                if (!customerAccountLogic.DebitCustomerAccount(customerAccount, amount + charge))
                    return false;
                //TODO: suppose to credit an atm charge GL for the charge so as to fulfil double entry
                //customerAccountLogic.CreditCustomerAccount(tellerPost.CustomerAccount, tellerPost.Amount);
                //tellerDAO.Update(teller);
               // Dao.Insert(tellerPost);
                NHibernateHelper.Commit();
                return true;
            }
            catch (Exception)
            {
                NHibernateHelper.Rollback();
                return false;
            }
        }

        public IList<CardTransaction> GetByOriginalDataElement(string orDateEl)
        {          
           return Dao.GetByOriginalDataElement(orDateEl);
        }

        public bool ProcessReversal(string orDateEl)
        {
            throw new NotImplementedException();
        }

        public bool ProcessReversal(CardTransaction cardTransaction, out CustomerAccount customerAccount)
        {
            //CardTransaction cardTransaction = GetByOriginalDataElement(orDateEl).
            //                            Where(x => x.MTI == "200").LastOrDefault();            
            customerAccount = customerAccountLogic.GetCustomerAccountByAccountNumber(Convert.ToInt64(cardTransaction.AccountNumber));
            if (cardTransaction.TransactionType == "01")
            {                
                return ProceesCardWithdrawalReversal(customerAccount, cardTransaction.Amount, cardTransaction.Charge);
            }
            else if (cardTransaction.TransactionType == "50")
            {
                CustomerAccount payeeAccount = customerAccountLogic.GetCustomerAccountByAccountNumber(Convert.ToInt64(cardTransaction.AccountNumber2));
                return ProcessFundTransferReversal(customerAccount, payeeAccount, cardTransaction.Amount, cardTransaction.Charge);
            }

            return false;
        }

        private bool ProceesCardWithdrawalReversal(CustomerAccount customerAccount, double amount, double charge)
        {
            try
            {
                //TODO: create a glaccount for the charge
                if (!glAccountLogic.DebitATMTillAccount(amount))
                    return false;
                if (!customerAccountLogic.ReverseCustomerAccountDebit(customerAccount, amount + charge))
                    return false;
                //customerAccountLogic.CreditCustomerAccount(tellerPost.CustomerAccount, tellerPost.Amount);
                //tellerDAO.Update(teller);
                // Dao.Insert(tellerPost);
                NHibernateHelper.Commit();
                return true;
            }
            catch (Exception)
            {
                NHibernateHelper.Rollback();
                return false;
            }
        }

        public bool ProcessFundTransfer(CustomerAccount payerAccount, CustomerAccount payeeAccount, double amount, double charge)
        {
            try
            {                
                if (!customerAccountLogic.DebitCustomerAccount(payerAccount, amount + charge))
                    return false;
                customerAccountLogic.CreditCustomerAccount(payeeAccount, amount);
                //TODO: suppose to credit an atm charge GL for the charge so as to fulfil double entry               
                NHibernateHelper.Commit();
                return true;
            }
            catch (Exception)
            {
                NHibernateHelper.Rollback();
                return false;
            }
        }

        private bool ProcessFundTransferReversal(CustomerAccount payerAccount, CustomerAccount payeeAccount, double amount, double charge)
        {
            try
            {
                //TODO: create a glaccount for the charge
                if (!customerAccountLogic.DebitCustomerAccount(payeeAccount, amount))
                    return false;
                if (!customerAccountLogic.ReverseCustomerAccountDebit(payerAccount, amount + charge))
                    return false;
                
                NHibernateHelper.Commit();
                return true;
            }
            catch (Exception)
            {
                NHibernateHelper.Rollback();
                return false;
            }
        }
    }
}
