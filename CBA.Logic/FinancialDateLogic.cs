using CBA.Core;
using CBA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Logic
{
    public class FinancialDateLogic : BaseLogic<FinancialDate, FinancialDateDAO>
    {
        public override bool Insert(FinancialDate obj, out string msg)
        {
            msg = "";
            if (GetById(1) != null) //Only one row can be inserted
                return false;
            return base.Insert(obj, out msg);
        }

    }
}
