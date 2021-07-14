using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CBA.Core
{
   
    public class MainAccount : Entity
    {
     
        [Display(Name="Branch Name")]
        [Required(ErrorMessage="{0} is required")]       
        public virtual AccountCategory Name { get; set; }
        
       
    }
}
