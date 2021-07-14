using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Core
{
    /// <summary>
    /// Base class for Savings and Current Account Management
    /// </summary>
    public class AccountConfiguration : Entity
    {
        [Required]
        [Display(Name = "Credit Interest Rate")]
        public virtual double CreditInterestRate{ get; set; }
        [Required]
        [Display(Name = "Minimum Balance")]
        public virtual double MinimumBalance { get; set; }
        [Required]
        [Display(Name = "Interest Expense GL Account")]
        public virtual GLAccount InterestExpenseGLAccount { get; set; }

    }
}
