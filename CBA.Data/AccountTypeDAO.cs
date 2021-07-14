using CBA.Core;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Data
{
    public class AccountTypeDAO : EntityDAO<AccountType>
    {
        public AccountType GetByName( AccountTypeEnum name)
        {
            AccountType accountType;
            
            accountType = Session.QueryOver<AccountType>().
                Where(x => x.Name == name).SingleOrDefault();
            return accountType;
        }

        public AccountType GetByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
