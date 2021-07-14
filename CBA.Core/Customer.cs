using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Core
{
    public class Customer : Person
    {
        [Display(Name = "Customer ID")]
        public virtual int CustomerID { get; set; }
        public virtual string Address { get; set; }
        public virtual Gender Gender { get; set; }
    }
}
