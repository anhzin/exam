using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankSystem.Common
{
    public class OperationErrorStatus
    {
        public enum TransactionError
        {
            AccountNotExit = 0,
            BalanceNotEnough = 1,
            AccountTargetNotExit = 0,
        }
    }
}
