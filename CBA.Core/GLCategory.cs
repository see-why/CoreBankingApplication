using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Core
{
    public class GLCategory : Entity
    {
        public virtual string Name { get; set; }
        public virtual MainAccount MainCategory { get; set; }
        public virtual string Description { get; set; }
    }
}
