using CBA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Mappings
{
    public class LoanAccountMap : EntityMap<LoanAccount>
    {
        public LoanAccountMap()
        {
            References(x => x.LinkedAccount);
            Map(x => x.Name).Not.Nullable();
            //Map(x => x.Name);
            Map(x => x.AccountNumber).Unique().Not.Nullable();
            Map(x => x.RepaymentInterval);
            Map(x => x.Duration);
            Map(x => x.PrincipalAmount);
            Map(x => x.InstallmentAmount);            
            Map(x => x.AmountRepaid);

            Map(x => x.LastPaidDate);
            Map(x => x.NextDueDate);
            Map(x => x.DueDate);

            Map(x => x.AccumulatedInterest);
            //Map(x => x.AccumulatedCharge);
            References(x => x.Status);
            References(x => x.Branch);
        }
    }
}
