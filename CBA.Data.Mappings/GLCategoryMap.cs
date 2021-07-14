using CBA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Mappings
{
    class GLCategoryMap : EntityMap<GLCategory>
    {
        public GLCategoryMap()
        {
            Map(x => x.Name).Unique().Not.Nullable();
           // Map(x => x.MainCategory).Not.Nullable();
            Map(x => x.Description).Not.Nullable();
            References(x => x.MainCategory).Cascade.All();
        }
        
    }
}
