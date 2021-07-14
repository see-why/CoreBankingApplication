using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Core
{
    public class LoanAccount : Entity
    {
        [Display(Name = "Linked Account")]
        public virtual CustomerAccount LinkedAccount { get; set; }
        [Display(Name = "Account Name")]
        public virtual string Name { get; set; }
        [Display(Name = "Account Number")]
        public virtual long AccountNumber { get; set; }
        [Display(Name = "Repayment Interval(days)")]
        public virtual int RepaymentInterval{ get; set; }//In days

        [Display(Name = "Duration(days)")]
        public virtual int Duration { get; set; }//In days

        [Display(Name = "Principal Amount")]
        public virtual double PrincipalAmount { get; set; }
        [Display(Name = "Installment Amount")]
        public virtual double InstallmentAmount { get; set; }
        public virtual double AmountRepaid { get; set; }
        public virtual DateTime LastPaidDate { get; set; }
        public virtual DateTime NextDueDate { get; set; }
        public virtual DateTime DueDate { get; set; }
        public virtual Double AccumulatedInterest { get; set; }
        //[Display(Name = "")]
        public virtual LoanStatus Status{ get; set; }
        public virtual Branch Branch { get; set; }
        
    }
}
