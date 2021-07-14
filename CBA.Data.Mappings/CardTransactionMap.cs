using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBA.Core;
using FluentNHibernate.Mapping;

namespace CBA.Mappings
{
    public class CardTransactionMap : EntityMap<CardTransaction>
    {
        public CardTransactionMap()
        {
            Map(x => x.CardHolderName);
            Map(x => x.AccountNumber);
            Map(x => x.AccountNumber2);
            Map(x=>x.CardPan);
            Map(x=>x.MTI);
            Map(x=>x.STAN);
            Map(x=>x.TransactionDate);
            Map(x=>x.TransactionType);
            Map(x=>x.Amount);
            Map(x=>x.Charge);
            Map(x=>x.ResponseCode);
            Map(x=>x.ResponseDescription);
            Map(x=>x.IsReversed);
            Map(x => x.OriginalDataElement);
        }
    }
}