using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankSystem.Models
{
    public class Transaction
    {
        public Guid ID { get; set; }
        public Guid AccountNumber { get; set; }
        public TransactionTypes? Type { get; set; }
        public decimal Amount { get; set; }
        public string Target { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public enum TransactionTypes
    {
        Deposite = 0,
        WithDraw = 1,
        Transfer = 2
    }
}
