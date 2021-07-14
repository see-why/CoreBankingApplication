using CBA.Core;
using CBA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Logic
{
    public class MainAccountLogic : BaseLogic<MainAccount, MainAccountDAO>
    {
        public MainAccount GetMainAccountByName(AccountCategory name)
        {
            return Dao.GetMainAccountByName(name);
        }

    }
}
