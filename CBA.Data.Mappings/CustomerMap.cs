using CBA.Core;
using FluentNHibernate.Mapping;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Mappings
{
    public class CustomerMap : PersonMap<Customer>
    {
        public CustomerMap()
        {
            References(x => x.Gender).Not.Nullable();
            Map(x => x.CustomerID).Unique().Not.Nullable();
            Map(x => x.Address);
            //Map(x => x.DateCreated);
            //Map(x => x.DateModified);
        }
    }
}
