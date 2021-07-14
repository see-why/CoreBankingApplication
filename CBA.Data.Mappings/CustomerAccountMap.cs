using CBA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Mappings
{
    class CustomerAccountMap : AccountMap<CustomerAccount>
    {
        public CustomerAccountMap()
        {
            Table("CustomerAccount");
           // SchemaAction.None();
            References(x => x.Customer);
            //Map(x => x.Name);
            Map(x => x.AccountNumber).Unique().Not.Nullable();
            References(x => x.AccountType);
            References(x => x.Branch);
            //Map(x => x.Balance);
            Map(x => x.IsActive);
            Map(x => x.AccumulatedInterest);
            Map(x => x.AccumulatedCharge);
            Map(x => x.LastInterestPaidDate);           
        }
    }
}
