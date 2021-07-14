using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CBA.Core
{
    public abstract class Person : Entity
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public virtual string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Your Last Name is too long")]
        public virtual string LastName { get; set; }

        [Display(Name = "Other Name")]
        public virtual string OtherNames { get; set; }

        [Display(Name = "Phone Number")]
        [Required]
        public virtual string PhoneNumber { get; set; }

        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email doesn't look like a valid email address.")]
        //[Remote("IsUserExistingWithEmail", "Validation")] 
        public virtual string Email { get; set; }

    }
}
