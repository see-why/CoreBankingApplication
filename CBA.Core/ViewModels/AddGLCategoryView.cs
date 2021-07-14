using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace CBA.Core.ViewModels
{
    public class AddGLCategoryView
    {
        [Required]
        [StringLength(50, ErrorMessage = "{0} should not exceed {1} characters")]
        [RegularExpression(@"^[a-zA-Z]+( [a-zA-z]+)*$", ErrorMessage = "{0} should consist of only alphabets and spaces")]
        public string Name { get; set; }

        [Required]
        [Display(Name="Main Category")]
        public int MainAccountCategoryID { get; set; }
       
        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "{0} should consist of {2} to {1} characters")]
        [RegularExpression(@"^[a-zA-Z]+( [a-zA-z]+)*$", ErrorMessage = "{0} should consist of only alphabets and spaces")]
        public string Description { get; set; }

    }
}
