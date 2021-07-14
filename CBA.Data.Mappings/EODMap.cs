using CBA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Mappings
{
    public class EODMap : EntityMap<EOD>
    {
        public EODMap()
        {
            Map(x => x.BusinessStatus);
            //References(x => x.SuperAdmin);
            Map(x => x.FinancialDate);            
        }
    }
}
