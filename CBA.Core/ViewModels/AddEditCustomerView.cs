using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace CBA.Core.ViewModels
{
    public class AddEditCustomerView
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, ErrorMessage = "{0} should not exceed {1} characters")]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "{0} should consist of only alphabets")]
        //[RegularExpression(@"/([a-zA-Z0-9-\s])/",ErrorMessage="First Name must be alphanumeric")]
        //[RegularExpression(@"/([a-zA-Z][0-9-\s]*)/",ErrorMessage="First Name must be alphanumeric")]
        public string FirstName { get; set; }

        
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "{0} should not exceed {1} characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "{0} should consist of only alphabets")]
        public string LastName { get; set; }

        [Display(Name = "Other Name")]
        [StringLength(50, ErrorMessage = "{0} should not exceed {1} characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "{0} should consist of only alphabets")]
        public virtual string OtherNames { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(50,MinimumLength=15, ErrorMessage = "{0} should consist of {2} to {1} characters")]
        //[RegularExpression(@"^[a-zA-Z]+( [a-zA-z]+)*$", ErrorMessage = "{0} should consist of only alphabets")]
        [RegularExpression(@"^[a-zA-Z0-9]+( [a-zA-z]+)*$", ErrorMessage = "{0} should consist of only alphabets")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        //[DataType(DataType.PhoneNumber)]
        [Phone]
        //[StringLength(20, MinimumLength = 7, ErrorMessage = "{0} should consist of {2} to {1} digits")]
        [RegularExpression(@"^(0?(-?\d{3})-?)?(\d{3})(-?\d{4})$",ErrorMessage="Invalid Phone number")]
        public virtual string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, ErrorMessage = "{0} should not exceed {1} characters")]
        public virtual string Email { get; set; }

        [Required]
        public int Gender { get; set; }
    }
}
