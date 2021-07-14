using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Core
{

    public enum AccountTypeEnum
    {
        SAVINGS = 1,
        CURRENT = 2,
        LOAN = 3
    }
    public enum Status
    {
        INACTIVE = 0,
        ACTIVE = 1
    }

    public enum GenderEnum
    {
        MALE = 1,
        FEMALE = 2
    }

    public enum AccountCategory
    {
        ASSET = 1,
        LIABILITY = 2,
        CAPITAL = 3,
        INCOME = 4,
        EXPENSE = 5
    }
    public enum Channel
    {
        ATM = 1,
        OVER_COUNTER=5     
    }

    public enum PostingType
    {
        DEPOSIT = 1,
        WITHDRAWAL = 2
    }
    public enum RolesEnum
    {
        ADMIN = 1,
        TELLER = 2
    }

    public enum LoanStatusEnum
    { 
        UNPAID = 1,
        PAID = 2,
        DEFAULTED = 3
    }

    public enum Business
    {
        OPEN  = 1,
        INBETWEEN = 2,
        CLOSED = 3
    }
}
