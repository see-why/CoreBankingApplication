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
    [Bind(Include="Name")]
    [Table("Branches")]
    public class Branch : Entity
    {
        
        //public virtual string Code { get; set; }
        
        [Display(Name="Branch Name")]
        [Required(ErrorMessage="{0} is required")]
        [StringLength(50, ErrorMessage = "{0} should not exceed {1} characters")]
        [RegularExpression(@"^[a-zA-Z]+( [a-zA-z0-9]+)*$", ErrorMessage = "{0} should consist of only alphabets, space and number")]
        //[Remote("IsBranchNameAvailable", "Validation")]        
        public virtual string Name { get; set; }
        [ScaffoldColumn(false)]
        public virtual Status Status { get; set; }
        [ScaffoldColumn(false)]
        public virtual bool IsBranchOpen { get; set; }
        [NotMapped]
        [ScaffoldColumn(false)]
        public virtual DateTime FinancialDate { get; set; }

        public virtual List<User> Users { get; set; }
    }
}
