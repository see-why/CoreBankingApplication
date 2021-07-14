using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace CBA.Core.ViewModels
{
    public class FindTellerView
    {
        
        [Display(Name = "User name")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "{0} should consist of only alphabets and numbers")]
        [StringLength(50, ErrorMessage = "{0} should not exceed {1} characters")]
        public string UserName { get; set; }
        
        [Display(Name = "Till Account Name")]
        [StringLength(50, ErrorMessage = "{0} should not exceed {1} characters")]
        [RegularExpression(@"^[a-zA-Z]+( [a-zA-z0-9]+)*$", ErrorMessage = "{0} should consist of only alphabets, space and number")]
        public string TillAccountName { get; set; }
        
    }
}
