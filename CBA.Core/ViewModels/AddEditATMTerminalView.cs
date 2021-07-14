using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace CBA.Core.ViewModels
{
    public class AddEditATMTerminalView
    {
        public int ID { get; set; }

        [Display(Name="Name")]
        [Required(ErrorMessage="{0} is required")]
        [StringLength(50, ErrorMessage = "{0} should not exceed {1} characters")]
        [RegularExpression(@"^[a-zA-Z]+( [a-zA-z0-9]+)*$", ErrorMessage = "{0} should consist of only alphabets, space and number")]
        //[Remote("IsBranchNameAvailable", "Validation")]        
        public string Name { get; set; }

        [Display(Name = "Terminal ID")]
        public int TerminalID { get; set; }
        public string Location { get; set; }

              

    }
}
