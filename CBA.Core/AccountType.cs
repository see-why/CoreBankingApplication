using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Core
{
    //public class AccountType : Entity
    //{
    //    //for savings account config
    //    public virtual string Name { get; set; } //can be savings, current or loan
    //    public virtual decimal CIR { get; set; }
    //    public virtual double MinimumBalance { get; set; }
    //    public virtual GLAccount InterestExpenseGLAccount { get; set; }

    //    //for current include these properties
    //    public virtual decimal COT { get; set; } //Commission on turnover
    //    public virtual GLAccount COTIncomeGLAccount { get; set; }

    //    //for loan use these properties
    //    public virtual decimal DIR { get; set; }
    //    public virtual GLAccount InterestIncomeAccount { get; set; }
    //    //public virtual GLAccount PrincipalLoanAccount { get; set; } //Source of disbursing loans

    //}
    public class AccountType : Entity 
    {
        public virtual AccountTypeEnum Name { get; set; }
    }
}
