using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO.Enum;


public enum ActionEnum
{
    None = 0,
    Insert = 1,
    Update = 2,
    Delete = 3
}

public enum filterConditionAndOrEnum
{
    AndCondition = 1,
    OrCondition = 2
}

public enum CreditDebit
{
    Credit = 0,
    Debit = 1
}