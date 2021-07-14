using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Core
{
    public class LoanAccountConfiguration : Entity
    {
        [Display(Name = "Debit Interest Rate")]
        public virtual double DebitInterestRate { get; set; }
        //public virtual double MinimumBalance { get; set; }
        [Display(Name = "Interest Income GL Account")]
        public virtual GLAccount InterestIncomeGLAccount { get; set; }
    }
}
