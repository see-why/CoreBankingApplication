using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace CBA.Core.ViewModels
{
    public class PostCustomerAccountView
    {        
        [Required]
        [Display(Name = "Transaction Type")]
        public int TransactionType { get; set; } //GL Account to credit

        [Required]
        //[Display(Name = "Transaction Type")]
        public int CustomerAccountID { get; set; } 

        [Required]       
        [Display(Name = "Amount")]
        [DataType(DataType.Currency)]
        [Range(200,2000000)]// Assumption
        public double Amount { get; set; }        
                
        [Required]
        [Display(Name = "Narration")]
        [DataType(DataType.MultilineText)]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "{0} should consist of {2} to {1} characters")]
        [RegularExpression(@"^[a-zA-Z]+( [a-zA-z]+)*$", ErrorMessage = "{0} should consist of only alphabets and spaces")]
        public string Narration { get; set; }
        //[Required]
        //[Display(Name = "Debit Narration")]
        //public string DebitNarration { get; set; }
       
    }
}
