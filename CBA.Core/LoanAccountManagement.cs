using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Core
{
    class LoanAccountManagement : Entity
    {
        public virtual double DebitInterestRate{ get; set; }
        public virtual GLAccount InterestIncomeAccount { get; set; }
    }
}
