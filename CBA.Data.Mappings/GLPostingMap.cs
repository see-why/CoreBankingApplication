using CBA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Mappings
{
    public class GLPostingMap : EntityMap<GLPost>
    {
        public GLPostingMap()
        {
            References(x => x.AccountDebited);
            Map(x => x.DebitAmount);
            Map(x => x.Narration);
            References(x => x.AccountCredited);
            Map(x => x.CreditAmount);
            //Map(x => x.CreditNarration);
            Map(x => x.TransactionDate);            
        }        
    }
}
