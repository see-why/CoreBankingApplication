using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace CBA.Core.ViewModels
{
    public class FindUserView
    {
        [Required]
        [Display(Name = "User name")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "{0} should consist of only alphabets and numbers")]
        [StringLength(50, ErrorMessage = "{0} should not exceed {1} characters")]
        public string UserName { get; set; }                
    }
}
