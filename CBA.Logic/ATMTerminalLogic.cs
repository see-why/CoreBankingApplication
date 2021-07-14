using CBA.Core;
using CBA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Logic
{
    public class ATMTerminalLogic : BaseLogic<ATMTerminal, ATMTerminalDAO>
    {
        public IList<Customer> Search(string text)
        {
            return Dao.Search(text);
        }        

        public IList<ATMTerminal> FindTerminalByName(string name)
        {
            return Dao.FindTerminalByName(name);
        }

        public bool IsTerminalNameAvailable(string name)
        {
            if (Dao.GetTerminalByName(name) != null)
            {
                return false;
            }
            return true; 
        }
    }
}
