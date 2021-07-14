using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CBA.Core
{
    [Bind(Exclude = "Branch")]
    public class User : Person
    {
        [Display(Name = "User Name")]
        [Required(ErrorMessage = "The {0} is required")]
        [StringLength(50)]
        [Remote("IsUserNameAvailable", "Validation")] //No two users should have the same password
        //[ScaffoldColumn(false)] hides a property from HTML helpers such as EditorForModel and DisplayForModel
        public virtual string UserName { get; set; }

        [Required(ErrorMessage = "The {0} is required")]
        [StringLength(50, MinimumLength = 3)]
        [DataType(DataType.Password)]
        public virtual string Password { get; set; }

        [Required(ErrorMessage = "The {0} is required")]
        [Display(Name = "Confirm Password")]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        [DataType(DataType.Password)]
        public virtual string ConfirmPassword { get; set; }
        public virtual Branch Branch { get; set; }
        public virtual bool IsSuperAdmin { get; set; }

    }
}
