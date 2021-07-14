using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace CBA.Core.ViewModels
{
    public class EditGLCategoryView
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "{0} should not exceed {1} characters")]
        [RegularExpression(@"^[a-zA-Z]+( [a-zA-z]+)*$", ErrorMessage = "{0} should consist of only alphabets and spaces")]
        public virtual string Name { get; set; }
       // public virtual AccountCategory Category { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "{0} should consist of {2} to {1} characters")]
        [RegularExpression(@"^[a-zA-Z]+( [a-zA-z]+)*$", ErrorMessage = "{0} should consist of only alphabets and spaces")]
        public virtual string Description { get; set; }
    }
}
