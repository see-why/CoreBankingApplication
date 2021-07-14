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

    public class LoanStatus : Entity
    {
     
        [Display(Name="Status")]
        //[Required(ErrorMessage="{0} is required")]       
        public virtual LoanStatusEnum Name { get; set; }
        
       
    }
}
