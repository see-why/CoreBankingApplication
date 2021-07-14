using CBA.Core;
using CBA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Logic
{
    public class AccountTypeBus : BaseLogic<AccountType, AccountTypeDAO>
    {
        public AccountType GetByName(string name)
        {
            return Dao.GetByName(name);
        }

        //allow EOD process
        public bool allowEOD(AccountType accountType)
        {
            string Name = accountType.Name.ToString();
            bool response = false;
            //switch (Name)
            //{ 
            //    case "savings":
            //        response = (accountType.CIR <= 0 || accountType.MinimumBalance <= 0 || 
            //            accountType.InterestExpenseGLAccount == null) ? false : true;
            //        break;
            //    case "current":
            //        response = (accountType.CIR <= 0 || accountType.MinimumBalance <= 0 || 
            //            accountType.InterestExpenseGLAccount == null || accountType.COT <=0
            //            || accountType.COTIncomeGLAccount == null) ? false : true;
            //        break;
            //    case "loan":
            //        response = (accountType.DIR <= 0 || accountType.InterestIncomeAccount == null
            //            //|| accountType.PrincipalLoanAccount == null
            //            ) ? false : true;
            //        break;
            //    default:
            //        response = false;
            //        break;
            //}
            return response;
        }
    }
}
