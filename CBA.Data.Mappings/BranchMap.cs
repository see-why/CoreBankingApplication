using CBA.Core;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Mappings
{
    public class BranchMap : EntityMap<Branch>
    {
        public BranchMap()
        {
            Table("Branches");
            SchemaAction.None();
            Map(x => x.Name).Unique().Not.Nullable();
           // Map(x => x.Code).Not.Nullable();
            Map(x => x.Status).CustomType<Status>();
           // Map(x => x.Status).Not.Nullable();
            Map(x => x.IsBranchOpen);
           //  Map(x => x.FinancialDate);                        
           
        }

    }
}
