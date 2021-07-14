using CBA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Mappings
{
    public class ATMTerminalMap : EntityMap<ATMTerminal>
    {
        public ATMTerminalMap()
        {
            Map(x => x.Name).Unique().Not.Nullable();           
            Map(x => x.TerminalID).Unique().Not.Nullable();
            Map(x => x.Location);                       
        }
    }
}
