using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace CBA.Core.ViewModels
{
    public class AddCurrentConfigView : AddSavingsConfigView
    {
        [Required]
        [Range(0,100,ErrorMessage="COT should range between 0 and 100")]
        //[Display(Name = "Credit Interest Rate")]
        public virtual double COT { get; set; } //Commission on turnover

        [Required]
        [Display(Name = "COT Income GL Account")]
        public virtual int COTIncomeGLAccountID { get; set; }

        //[Required]
        //[Range(0, 10000, ErrorMessage = "Minimum Balance should not be greater than 10000")]
        //[Display(Name = "Minimum Balance")]
        //public virtual double MinimumBalance { get; set; }
        //[Required]
        //[Display(Name = "Interest Expense GL Account")]
        //public virtual int InterestExpenseGLAccountID { get; set; }
              

    }
}
