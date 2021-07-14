using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace CBA.Core.ViewModels
{
    public class EditCustomerAccountView
    {
        //Hidden field
        [Required]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Account Name")]
        [StringLength(50, ErrorMessage = "{0} should not exceed {1} characters")]
        [RegularExpression(@"^[a-zA-Z]+( [a-zA-z]+)*$", ErrorMessage = "{0} should consist of only alphabets")]
        public string AccountName { get; set; }        

        [Required]
        [Display(Name = "Branch")]
        public int BranchID { get; set; }
              

    }
}
