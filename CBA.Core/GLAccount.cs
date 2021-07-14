using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Core
{
    public class GLAccount : Account
    {
        [Display(Name= "GL Code")]
        public virtual int Code { get; set; }
        [Display(Name = "GL Category")]
        public virtual GLCategory GLCategory { get; set; }
        public virtual Branch Branch { get; set; }        
    }
}
