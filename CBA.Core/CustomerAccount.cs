using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Core
{
    public class CustomerAccount : Account
    {
        public virtual Customer Customer { get; set; }
       // public virtual String Name { get; set; }
        [Display(Name = "Account Number")]
        public virtual long AccountNumber { get; set; }
        public virtual Branch Branch { get; set; }
        public virtual AccountType AccountType { get; set; }
        public virtual bool IsActive { get; set; }
        //public virtual double Balance { get; set; }
        public virtual double AccumulatedInterest { get; set; }
        public virtual double AccumulatedCharge { get; set; }// e.g COT, atm fees etc
        public virtual DateTime LastInterestPaidDate { get; set; }

    }
}
