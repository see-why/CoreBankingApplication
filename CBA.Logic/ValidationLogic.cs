using CBA.Core;
using CBA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Logic
{

    public class ValidationLogic 
    {
        ValidationDAO dao = new ValidationDAO();

        public static bool IsInteger(string input) 
        {
            int number;
            if (int.TryParse(input, out number))
                return true;

            return false;
            
        }

        //checks if it is a non negative and zero long number. Also if it starts with 1 or 2 for
        //savings and current account
        public static bool IsValidAccountFormat(string input)
        {
            char firstDigit = input[0];
            //AccountTypeEnum a = 0;
            //bool startsWithValidAccountType = Enum.TryParse<AccountTypeEnum>(firstDigit.ToString(),out a);
            //if (firstDigit != '1' && firstDigit != '2')
            //{
                
            //}
            bool startsWithValidAccountType = firstDigit == '1' || firstDigit == '2';

            long number;
            if (long.TryParse(input, out number) && number>0 && startsWithValidAccountType )
                return true;

            return false;
        }
    }
}
