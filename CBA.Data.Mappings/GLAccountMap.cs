using CBA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Mappings
{
    public class GLAccountMap : AccountMap<GLAccount>
    {
        public GLAccountMap()
        {
            Map(x => x.Code).Unique().Not.Nullable();
           // Map(x => x.Name).Not.Nullable();
            References(x => x.GLCategory).Cascade.All();
            References(x => x.Branch);
           // Map(x => x.Balance);            
        }
    }
}
