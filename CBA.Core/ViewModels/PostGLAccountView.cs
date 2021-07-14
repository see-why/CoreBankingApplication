using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace CBA.Core.ViewModels
{
    public class PostGLAccountView
    {        
        [Required]
        [Display(Name = "Credit Account")]
        public int AccountCreditedID { get; set; } //GL Account to credit
        [Required]       
        [Display(Name = "Credit Amount")]
       // [DataType(DataType.Currency)]
        [Range(200, 2000000)]// Assumption
        public double CreditAmount { get; set; }        
       
        [Required]
        [Display(Name = "Debit Account")]
        public int AccountDebitedID { get; set; } //GL Account to debit

        [Required]
        [Compare("CreditAmount")]
        [Display(Name = "Debit Amount")]
        [Range(200, 2000000)]// Assumption
        public double DebitAmount { get; set; }

        [Required]
        [Display(Name = "Narration")]
        [DataType(DataType.MultilineText)]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "{0} should consist of {2} to {1} characters")]
        [RegularExpression(@"^[a-zA-Z]+( [a-zA-z]+)*$", ErrorMessage = "{0} should consist of only alphabets and spaces")]
        public string Narration { get; set; }        
       
    }
}
