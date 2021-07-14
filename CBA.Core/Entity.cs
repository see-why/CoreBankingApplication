using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CBA.Core
{
    public abstract class Entity
    {
        [Key]
        [ScaffoldColumn(false)] 
        public virtual int ID { get; protected set; }
        [ScaffoldColumn(false)] 
        public virtual DateTime DateCreated { get; set; }
  
        [ScaffoldColumn(false)] 
        public virtual DateTime DateModified { get; set; }
    }
}
