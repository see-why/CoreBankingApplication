using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace CBA.Core.ViewModels
{
    public class FindCustomerView
    {       

        [StringLength(50, ErrorMessage = "{0} should not exceed {1} characters")]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "{0} should consist of only alphabets")]
        public string FirstName { get; set; } 

       [StringLength(50, ErrorMessage = "{0} should not exceed {1} characters")]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "{0} should consist of only alphabets")]
        public string LastName { get; set; }
        
    }
}
