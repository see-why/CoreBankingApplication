using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Core
{
    public class EOD : Entity
    {
        public virtual Business BusinessStatus { get; set;}
       // public virtual User SuperAdmin { get; set; }
        public virtual DateTime FinancialDate { get; set; }

    }
}
