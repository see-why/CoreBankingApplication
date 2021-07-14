using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace CBA.Core.ViewModels
{
    public class AddLoanConfigView
    {
        [Required]
        [Range(1,100,ErrorMessage="Interest rate should range between 1% and 100%")]
        [Display(Name = "Debit Interest Rate")]
        public virtual double DebitInterestRate { get; set; }
        //public virtual double MinimumBalance { get; set; }
        [Required]
        [Display(Name = "Interest Income GL Account")]
        public virtual int InterestIncomeGLAccountID { get; set; }
                     

    }
}
