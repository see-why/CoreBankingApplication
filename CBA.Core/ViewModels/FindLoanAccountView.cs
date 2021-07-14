using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace CBA.Core.ViewModels
{
    public class FindLoanAccountView
    {
        //public int ID { get; set; }
        [Display(Name = "Account Number")]
        //[Required(ErrorMessage = "{0} is required")]
        [StringLength(11,MinimumLength=11,ErrorMessage="{0} has to be {1} digits")]       
        [RegularExpression(@"\d+", ErrorMessage = "{0} should consist of only numbers")]
        public string AccountNumber { get; set; }

        [StringLength(50, ErrorMessage = "{0} should not exceed {1} characters")]
        [Display(Name = "Account Name")]
        [RegularExpression(@"^[a-zA-Z]+( [a-zA-z]+)*$", ErrorMessage = "{0} should consist of only alphabets")]
        public string AccountName { get; set; } 

       
        
    }
}
