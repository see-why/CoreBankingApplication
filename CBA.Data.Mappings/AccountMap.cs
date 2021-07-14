using CBA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Mappings
{
    public class AccountMap<T> : EntityMap<T> where T:Account
    {
        public AccountMap()
        {
           // SchemaAction.None();       
            Map(x => x.Name).Not.Nullable();           
            Map(x => x.Balance);            
        }
    }
}
