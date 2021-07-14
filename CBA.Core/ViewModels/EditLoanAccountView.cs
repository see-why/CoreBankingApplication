using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace CBA.Core.ViewModels
{
    public class EditLoanAccountView
    {
        //Hidden field
        [Required]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Account Name")]
        [StringLength(50, ErrorMessage = "{0} should not exceed {1} characters")]
        [RegularExpression(@"^[a-zA-Z]+( [a-zA-z]+)*$", ErrorMessage = "{0} should consist of only alphabets")]
        public string AccountName { get; set; }        

        [Required]
        [Display(Name = "Branch")]
        public int BranchID { get; set; }

        [Required]
        [Display(Name = "Repayment Interval")]
        [Range(1, 60, ErrorMessage = "{0} should not exceed 60 days")]
        public int RepaymentInterval { get; set; } //In days

        [Required]
        [Display(Name = "Duration(days)")]
        [Range(1, 730, ErrorMessage = "{0} should not exceed 60 days")]
        public int Duration { get; set; } //In days
        //[Required]
        //[Display(Name = "Installment Amount")]
        //[Range(20000, 6000000)]
        //public double InstallmentAmount { get; set; }
              

    }
}
