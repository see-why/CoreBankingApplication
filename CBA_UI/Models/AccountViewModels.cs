using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CBA.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    public class EditUserViewModel
    {       
        public string Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "{0} should not exceed {1} characters")]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "{0} should consist of only alphabets")]

        public virtual string FirstName { get; set; }
        
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, ErrorMessage = "{0} should not exceed {1} characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "{0} should consist of only alphabets")]

        public virtual string LastName { get; set; }

        [Display(Name = "Other Name")]
        [StringLength(50, ErrorMessage = "{0} should not exceed {1} characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "{0} should consist of only alphabets")]

        public virtual string OtherNames { get; set; }

        [Display(Name = "Phone Number")]
        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(0?(-?\d{3})-?)?(\d{3})(-?\d{4})$", ErrorMessage = "Invalid Phone number")]
        public virtual string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        //[RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email doesn't look like a valid email address.")]
        //[Remote("IsUserExistingWithEmail", "Validation")] 
        public virtual string Email { get; set; }
        [Required]
        [Display(Name = "Branch")]
        public int BranchID { get; set; }
        public bool IsSuperAdmin
        {
            get;
            set;
        }
    }

        public class LoginViewModel
        {
            [Required]
            [Display(Name = "User name")]
            [StringLength(50, ErrorMessage = "{0} should not exceed {1} characters")]
            //[RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "{0} should consist of only alphabets and numbers")]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public class RegisterViewModel
        {
            [Required]
            [Display(Name = "User name")]
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "{0} should consist of only alphabets and numbers")]
            [StringLength(50, ErrorMessage = "{0} should not exceed {1} characters")]
            //[Remote("IsUserNameAvailable", "Validation")] 
            public string UserName { get; set; }

            [Required]
            [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "{0} should consist of only alphabets")]
            [StringLength(50, ErrorMessage = "{0} should not exceed {1} characters")]
            [Display(Name = "First Name")]
            public virtual string FirstName { get; set; }

            [Display(Name = "Last Name")]
            [Required(ErrorMessage = "Last name is required")]
            [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "{0} should consist of only alphabets")]
            [StringLength(50, ErrorMessage = "{0} should not exceed {1} characters")]
            public virtual string LastName { get; set; }

            [Display(Name = "Other Name")]
            [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "{0} should consist of only alphabets")]
            [StringLength(50, ErrorMessage = "{0} should not exceed {1} characters")]
            public virtual string OtherNames { get; set; }

            [Display(Name = "Phone Number")]
            [Required]
            [DataType(DataType.PhoneNumber)]
            [RegularExpression(@"^(0?(-?\d{3})-?)?(\d{3})(-?\d{4})$", ErrorMessage = "Invalid Phone number")]
            public virtual string PhoneNumber { get; set; }

            [Required]
            [DataType(DataType.EmailAddress)]
            //[RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email doesn't look like a valid email address.")]
            //[Remote("IsUserExistingWithEmail", "Validation")] 
            public virtual string Email { get; set; }

            //[Required]
            //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
            //[DataType(DataType.Password)]
            //[Display(Name = "Password")]
            //public string Password { get; set; }

            //[DataType(DataType.Password)]
            //[Display(Name = "Confirm password")]
            //[System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            //public string ConfirmPassword { get; set; }
            [Required]
            [Display(Name = "Branch")]
            public int BranchID { get; set; }
            public bool IsSuperAdmin { get; set; }

            [Required]
            [Display(Name = "Role")]
            public string Role { get; set; }
        }
    }

