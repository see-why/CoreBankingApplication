using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace CBA.Core.ViewModels
{
    public class AddGLAccountView
    {
        [Required]
        [StringLength(50, ErrorMessage = "{0} should not exceed {1} characters")]
        [RegularExpression(@"^[a-zA-Z]+( [a-zA-z0-9]+)*$", ErrorMessage = "{0} should consist of only alphabets, space and number")]
        public string Name { get; set; }

        [Required]       
        [Display(Name = "GL Category")]
        public int CategoryID { get; set; }

        [Required]
        [Display(Name = "Branch")]
        public int BranchID { get; set; }
              

    }
}
