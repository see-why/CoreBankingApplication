using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Core
{
    public class GLPost : Entity
    {
        [Display(Name = "Credited GLAccount")]
        public virtual GLAccount AccountCredited { get; set; } //GL Account to credit
        [Display(Name = "Credit Amount")]
        public virtual double CreditAmount { get; set; }
       // public virtual double Amount { get; set; }
        //[Display(Name = "Credit Narration")]
        //public virtual string CreditNarration { get; set; }
        [Display(Name = "Debited GLAccount")]
        public virtual GLAccount AccountDebited { get; set; } //GL Account to debit
        [Display(Name = "Debit Amount")]
        public virtual double DebitAmount { get; set; }
        [Display(Name = "Narration")]
        public virtual string Narration { get; set; }

        //[Display(Name = "Debit Narration")]
        //public virtual string DebitNarration { get; set; }

        [Display(Name = "Transaction Date")]
        public virtual DateTime TransactionDate { get;set; }//This uses the current financial date
    }
}
