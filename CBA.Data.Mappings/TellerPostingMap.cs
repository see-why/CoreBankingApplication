using CBA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Mappings
{
    public class TellerPostingMap : EntityMap<TellerPosting>
    {
        public TellerPostingMap()
        {
            References(x => x.CustomerAccount);
            References(x => x.Teller);
            Map(x => x.PostingType);
            Map(x => x.Amount);
            Map(x => x.Narration);
            Map(x => x.FinancialDate);            
        }
    }
}
