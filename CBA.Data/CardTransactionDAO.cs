using CBA.Core;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Data
{
    public class CardTransactionDAO : EntityDAO<CardTransaction>
    {
        CardTransaction cardTransaction= new CardTransaction();
        IList<CardTransaction> cardTransactionList = null;

        public IList<CardTransaction> GetByOriginalDataElement(string orDateEl)
        {
            cardTransactionList = Session.QueryOver<CardTransaction>()
               .Where(x => x.OriginalDataElement.IsInsensitiveLike(orDateEl, MatchMode.Exact)).List();
            return cardTransactionList;
        }
    }
}
