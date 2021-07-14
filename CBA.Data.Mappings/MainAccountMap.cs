using CBA.Core;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Mappings
{
    public class MainAccountMap : EntityMap<MainAccount>
    {
        public MainAccountMap()
        {
                   
            Map(x => x.Name).Unique().Not.Nullable();                                             
        }

    }
}
