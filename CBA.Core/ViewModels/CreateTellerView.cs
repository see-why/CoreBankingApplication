using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace CBA.Core.ViewModels
{
    public class CreateTellerView
    {
        [Required]
        [Display(Name = "Select User")]
        public string UserID { get; set; }

        [Required]       
        [Display(Name = "Select a Till Account")]
        public int TillAccountID { get; set; }

              

    }
}
