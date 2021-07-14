using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBA.FEP.MessageFields;
using CBA.Core;
using Trx.Messaging.Iso8583;
using CBA.Data;
using CBA.Logic;

namespace CBA.FEP.SwitchTransaction
{
    //************Things to do***************
    //get connection to sinknode
    //Process CardPan - BIN, acct no, transaction type... then process transaction - /check/validate balance, debit customer(withdrawal)
    //Return Response
    //Logs steps/transactions to log file and db.
    //Add a ViewCardtransactions page to CBA.

    public class CardTransactionProcessor
    {
        CustomerAccountLogic customerAccLogic = new CustomerAccountLogic();
        CardTransactionLogic cardTransactionLogic = new CardTransactionLogic();
        
        public Iso8583Message ProcessCardTransaction(Iso8583Message isoMessage)
        {
            Iso8583Message IsoMessage = isoMessage;
            string AcctNum = IsoMessage.Fields[FieldNos.F102_Account1].Value.ToString();
            string TrxnType = IsoMessage.Fields[FieldNos.F3_ProcCode].Value.ToString().Substring(0, 2);

            CustomerAccount custAccount = customerAccLogic.GetCustomerAccountByAccountNumber(Convert.ToInt64(AcctNum));
            if (custAccount == null)
            {
                IsoMessage.Fields.Add(39,"21");//No Action taken
                return IsoMessage;
            }

            switch (TrxnType)
            {
                case "31":  //ProcessBalanceEnqiury
                    double balance = custAccount.Balance;
                    IsoMessage.Fields.Add(FieldNos.F54_AdditionalAmounts, Utils.FormatNumber(balance));
                    isoMessage.Fields.Add(39, "00");
                    //IsoMessage.Fields.Add(FieldNos.F54_AdditionalAmounts, string.Format("0001566C{0}0002566C{1}",
                    //    balance.ToString().PadLeft(12, '0'), balance.ToString().PadLeft(12, '0')));
                    break;

                case "01":  //Process Cash Withdrawal
                    ProcessCardWithdrawal(custAccount, IsoMessage);
                    break;
                case "50":  //Process Fund Transfer
                    ProcessFundTransfer(custAccount, IsoMessage);
                    break;

            }
            return IsoMessage;
        }

        private Iso8583Message ProcessFundTransfer(CustomerAccount payerAccount, Iso8583Message isoMesg)
        {
            Iso8583Message responseMessage = isoMesg;
            string payeeAcctNum = isoMesg.Fields[FieldNos.F103_Account2].Value.ToString();

            CustomerAccount payeeAccount = customerAccLogic.GetCustomerAccountByAccountNumber(Convert.ToInt64(payeeAcctNum));
            if (payeeAccount == null || payeeAccount.AccountNumber == payerAccount.AccountNumber)
            {
                responseMessage.Fields.Add(39, "21");//No Action taken
                return responseMessage;
            }
            double amount = new ISO8583DataExtractor(responseMessage).Amount / 100;
            double fee = Convert.ToDouble(isoMesg.Fields[28].Value.ToString());
            if (!customerAccLogic.IsDeductable(payerAccount, amount + fee))
            {
                responseMessage.Fields.Add(39, "51"); //Insufficient funds
            }
            else if (cardTransactionLogic.ProcessFundTransfer(payerAccount,payeeAccount, amount, fee))
            {
                responseMessage.Fields.Add(39, "00");
                //responseMessage.Fields.Add(FieldNos.F54_AdditionalAmounts, customerAccount.Balance.ToString());
            }
            
            //else
            //{

            //}
            responseMessage.Fields.Add(FieldNos.F54_AdditionalAmounts, Utils.FormatNumber(payerAccount.Balance));
            return responseMessage;
        }

        public Iso8583Message ProcessCardWithdrawal(CustomerAccount customerAccount, Iso8583Message isoMesg)
        {
            Iso8583Message responseMessage = isoMesg;
            double amount = new ISO8583DataExtractor(responseMessage).Amount/100;
            double fee =  Convert.ToDouble(isoMesg.Fields[28].Value.ToString());
            if (!customerAccLogic.IsDeductable(customerAccount, amount+fee))
            {
               responseMessage.Fields.Add(39,"51"); //Insufficient funds
            }            
            else if (cardTransactionLogic.ProcessCardWithDraw(customerAccount, amount, fee))
            {
                responseMessage.Fields.Add(39,"00");
                //responseMessage.Fields.Add(FieldNos.F54_AdditionalAmounts, customerAccount.Balance.ToString());
            }
            //else
            //{

            //}
            responseMessage.Fields.Add(FieldNos.F54_AdditionalAmounts, Utils.FormatNumber(customerAccount.Balance));
            return responseMessage;
        }
        
        //Reversal
        public Iso8583Message ProcessReversal(Iso8583Message isoMsg)
        {
            var glAccountLogic = new GLAccountLogic();
            //var customerAccLogic = new CustomerAccountService();
            string orDateEl = isoMsg.Fields[90].ToString();
            //string orDateEl = isoMsg.Fields[90].ToString().Remove(0, 23);
            CustomerAccount customerAccount = null;
            CardTransaction cardTransaction = cardTransactionLogic.GetByOriginalDataElement(orDateEl).
                                        Where(x => x.MTI == "210" && x.IsReversed == false).LastOrDefault();
            if (cardTransaction == null)
            {
                isoMsg.Fields.Add(FieldNos.F39_ResponseCode, "00");//everything is okay the transaction was never processed                
            }
            else if (cardTransactionLogic.ProcessReversal(cardTransaction, out customerAccount))
            {
                cardTransaction.IsReversed = true;
                cardTransactionLogic.Update(cardTransaction);
                isoMsg.Fields.Add(FieldNos.F39_ResponseCode, "00");
                isoMsg.Fields.Add(FieldNos.F54_AdditionalAmounts, Utils.FormatNumber(customerAccount.Balance));
            }

            //CustomerAccount custAcct = customerAccLogic.GetCustomerAccountByAccountNumber(Convert.ToInt64(cardTransaction.AccountNumber));
            
            //GLAccount atmGl = glService.LoadByGLAccountName("ATM Till");

            //glService.DebitGLAccount(atmGl, (cardTrxn.Amount + cardTrxn.Charge));
            //customerAccLogic.CreditCustomerAccount(custAcct, (cardTrxn.Amount + cardTrxn.Charge));         

            //isoMsg.Fields.Add(FieldNos.F54_AdditionalAmounts, (Convert.ToInt64(customerAccount.Balance)).ToString());
            return isoMsg;
        }

        //Log Card Transaction 
        public void LogCardTrxnToCBA(Iso8583Message IsoMessage)
        {
            string mti = IsoMessage.MessageTypeIdentifier.ToString();

            string responseCode = string.Empty;

            try { responseCode = IsoMessage.Fields[39].Value.ToString(); }
            catch { }

            double fee = 0;
            try { fee = Convert.ToDouble(IsoMessage.Fields[28].ToString()); }
            catch { }
            //string responseCode = mti == "210" || mti == "430" ? IsoMessage.Fields[39].Value.ToString() : null;
            //string responseCode = !string.IsNullOrWhiteSpace(IsoMessage.Fields[39].Value.ToString()) ? 
                //                                              IsoMessage.Fields[39].Value.ToString() : string.Empty;

            CustomerAccount custAcc = customerAccLogic.GetCustomerAccountByAccountNumber(Convert.ToInt64(IsoMessage.Fields[102].ToString()));

            var cTrxn = new CardTransaction();

            cTrxn.AccountNumber = custAcc != null ? custAcc.AccountNumber.ToString() : string.Empty;
            cTrxn.Amount = Convert.ToDouble(IsoMessage.Fields[4].Value)/100;
            cTrxn.CardHolderName = custAcc.Name;
            cTrxn.CardPan = IsoMessage.Fields[2].Value.ToString();
            cTrxn.Charge = fee;
            cTrxn.IsReversed = mti == "430" && responseCode == "00"? true : false; 
            cTrxn.MTI = mti;

            string orDataElt = IsoMessage.Fields[90].ToString();
            int length = orDataElt.Length;
            cTrxn.OriginalDataElement = orDataElt; // orDataElt.Length > 19 ? orDataElt.Remove(0, (length - 19)) : orDataElt;

            cTrxn.ResponseCode = responseCode;
            cTrxn.ResponseDescription = !string.IsNullOrWhiteSpace(responseCode) ?
                                           ResponseDescriptions.SingleOrDefault(x => x.Key.ToString() == responseCode).Value : string.Empty;
            //cTrxn.ResponseDescription = !string.IsNullOrWhiteSpace(responseCode) ?
            //                               ResponseDescriptor.GetResponseDescription(responseCode) : string.Empty;
            cTrxn.STAN = IsoMessage.Fields[11].ToString();
            cTrxn.TransactionDate = DateTime.UtcNow;
            string transactionType = IsoMessage.Fields[3].Value.ToString().Substring(0, 2);
            cTrxn.TransactionType = transactionType;
            cTrxn.AccountNumber2 = transactionType == "50" ? IsoMessage.Fields[103].Value.ToString() : null;

            string msg = string.Empty;
            cardTransactionLogic.Insert(cTrxn, out msg);
            Console.WriteLine("Card Transaction Log: {0}", msg);
        }

        public readonly Dictionary<string, string> ResponseDescriptions = new Dictionary<string, string>
           {
               {"00", "Approve by Financial Institution"},
               {"01", "Refer to card issuer"},
               {"02", "Refer to card issuer, special condition"},
               {"03", "Invalid Merchant"},
               {"04", "Pick-up card"},
               {"05", "Do Not Honor"},
               {"06", "Error"},
               {"07", "Pick-Up Card, Special Condition"},
               {"08", "Honor with Identification"},
               {"09", "Request in Progress"},
               {"10", "Approved by Financial Institution, Partial"},
               {"11", "Approved by Financial Institution, VIP"},
               {"12", "Invalid Transaction"},
               {"13", "Invalid Amount"},
               {"14", "Invalid Card Number"},
               {"15", "No Such issuer"},
               {"16", "Approved by Financial Institution, Update Track 3"},
               {"17", "Customer Cancellation"},
               {"18", "Customer Dispute"},
               {"19", "Re-enter Transaction"},
               {"20", "Invalid Response from Financial Institution"},
               {"21", "No Action Taken by Financial Institution"},
               {"22", "Suspected Malfunction"},
               {"23", "Unacceptable Transaction Fee"},
               {"24", "File Update not Supported"},
               {"25", "Unable to Locate Record"},
               {"26", "Duplicate Record"},
               {"27", "File Update Field Edit Error"},
               {"28", "File Update File Locked"},
               {"29", "File Update Failed"},
               {"30", "Format Error"},
               {"31", "Bank Not Supported"},
               {"32", "Completed Partially by Financial Institution"},
               {"33", "Expired Card, Pick-Up"},
               {"34", "Suspected Fraud, Pick-Up"},
               {"35", "Contact Acquirer, Pick-Up"},
               {"36", "Restricted Card, Pick-Up"},
               {"37", "Call Acquirer Security, Pick-Up"},
               {"38", "PIN Tries Exceeded, Pick-Up"},
               {"39", "No Credit Account"},
               {"40", "Function not Supported"},
               {"41", "Lost Card, Pick-Up"},
               {"42", "No Universal Account"},
               {"43", "Stolen Card, Pick-Up"},
               {"44", "No Investment Account"},
               {"51", "Insufficient Funds"},
               {"52", "No Check Account"},
               {"53", "No Savings Account"},
               {"54", "Expired Card"},
               {"55", "Incorrect PIN"},
               {"56", "No Card Record"},
               {"57", "Transaction not Permitted to Cardholder"},
               {"58", "Transaction not Permitted on Terminal"},
               {"59", "Suspected Fraud"},
               {"60", "Contact Acquirer"},
               {"61", "Exceeds Withdrawal Limit"},
               {"62", "Restricted Card"},
               {"63", "Security Violation"},
               {"64", "Original Amount Incorrect"},
               {"65", "Exceeds withdrawal frequency"},
               {"66", "Call Acquirer Security"},
               {"67", "Hard Capture"},
               {"68", "Response Received Too Late"},
               {"75", "PIN tries exceeded"},
               {"76", "Reserved for Future Postilion Use"},
               {"77", "Intervene, Bank Approval Required"},
               {"78", "Intervene, Bank Approval Required for Partial Amount"},
               {"90", "Cut-off in Progress"},
               {"91", "Issuer or Switch Inoperative"},
               {"92", "Routing Error"},
               {"93", "Violation of law"},
               {"94", "Duplicate Transaction"},
               {"95", "Reconcile Error"},
               {"96", "System Malfunction"},
               {"98", "Exceeds Cash Limit"},
           };
    }
}
