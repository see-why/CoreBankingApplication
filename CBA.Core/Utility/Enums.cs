using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utility
{
    public enum Status
    {
        Inactive = 0,
        Active = 1
    }

    public enum Gender
    {
        Male = 1,
        Female = 2
    }

    public enum AccountCategory
    {
        Asset = 1,
        Liability = 2,
        Capital = 3,
        Income = 4,
        Expense = 5
    }

    public enum PostingType
    {
        Deposit = 1,
        Withdrawal = 2
    }

    public enum LoanStatus
    { 
        Paid = 1,
        Unpaid = 2,
        Breeched = 3
    }

    public enum Business
    {
        Open  = 1,
        Inbetween = 2,
        Closed = 3
    }
}
