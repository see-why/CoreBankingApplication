using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Core
{
    public class Teller : Entity
    {
        [Display(Name = "User")]
        public virtual User User { get; set; }
        [Display(Name = "Till")]
        public virtual GLAccount TillAccount { get; set; }
        
    }
}
