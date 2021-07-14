using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Core
{
    public class LoanMgt : Entity
    {        
        //public virtual long AccountNumber { get; set; }
        public virtual CustomerAccount LinkedAccount { get; set; }
        public virtual int Terms { get; set; }
        public virtual double InitialAmount { get; set; }
        public virtual double Amount { get; set; }
        public virtual DateTime DueDate { get; set; }
        public virtual Double AccumulatedInterest { get; set; }
        public virtual Decimal DIR { get; set; }
        public virtual LoanStatus LoanStatus { get; set; }
    }
}
