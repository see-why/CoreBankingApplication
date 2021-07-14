using CBA.Core;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Data
{
    public class MainAccountDAO : EntityDAO<MainAccount>
    {
        MainAccount mainAccount = new MainAccount();
        public MainAccount GetMainAccountByName(AccountCategory name)
        {
            mainAccount = Session.QueryOver<MainAccount>()
                .Where(x => x.Name == name).SingleOrDefault();
            return mainAccount;
        }
    }
}
