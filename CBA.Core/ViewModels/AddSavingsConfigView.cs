using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace CBA.Core.ViewModels
{
    public class AddSavingsConfigView
    {
        [Required]
        [Range(1,100,ErrorMessage="Interest rate should range between 1% and 100%")]
        [Display(Name = "Credit Interest Rate")]
        public virtual double CreditInterestRate { get; set; }
        [Required]
        [Range(0, 10000, ErrorMessage = "Minimum Balance should not be greater than 10000")]
        [Display(Name = "Minimum Balance")]
        public virtual double MinimumBalance { get; set; }
        [Required]
        [Display(Name = "Interest Expense GL Account")]
        public virtual int InterestExpenseGLAccountID { get; set; }
              

    }
}
