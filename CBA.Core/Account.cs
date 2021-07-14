using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Core
{
    public class Account : Entity
    {
        [Display(Name = "Account Name")]
        public virtual string Name { get; set; }        
        public virtual double Balance { get; set; }
    }
}
