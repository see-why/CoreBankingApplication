using CBA.Core;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Mappings
{
    public class GenderMap : EntityMap<Gender>
    {
        public GenderMap()
        {                   
            Map(x => x.Name).Unique().Not.Nullable();                                             
        }

    }
}
