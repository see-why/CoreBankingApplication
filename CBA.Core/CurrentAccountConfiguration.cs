using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Core
{
    public class CurrentAccountConfiguration : AccountConfiguration
    {
        public virtual double COT { get; set; } 
        [Display(Name = "COT Income GL Account")]
        public virtual GLAccount COTIncomeGLAccount { get; set; }
    }
}
