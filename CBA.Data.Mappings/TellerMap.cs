using CBA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Mappings
{
    public class TellerMap : EntityMap<Teller>
    {
        public TellerMap() {
            References(x => x.User);
            References(x => x.TillAccount);            
        }
    }
}
