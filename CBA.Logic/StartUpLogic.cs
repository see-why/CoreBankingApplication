using CBA.Core;
using CBA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Logic
{
    public class StartUpLogic
    {
        public static void SetCurrentBuisnessStatus() 
        {
            EODLogic eodLogic = new EODLogic();
            EOD eod = eodLogic.GetById(1);
            if (eod.BusinessStatus == Business.OPEN) 
            {
                
            }
        }
    }
}
