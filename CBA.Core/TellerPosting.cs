using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Core
{
    public class TellerPosting : Entity
    {
        public virtual CustomerAccount CustomerAccount { get; set; }
        public virtual Teller Teller { get; set; }
        public virtual PostingType PostingType { get; set; }
        public virtual double Amount { get; set; }
        public virtual string Narration { get; set; }
        public virtual DateTime FinancialDate { get; set; }
    }
}
