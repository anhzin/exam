using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BankSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankSystem.Services
{
    public interface ITransactionRepository : IDisposable
    {
        IEnumerable<Transaction> GetTransactions();
        Transaction GetTransactionByID(Guid transactiondID);
        void InsertTransaction(Transaction transaction);
        void DeleteTransaction(Guid transactiondID);
        void UpdateTransaction(Transaction transaction);
        void Save();
    }
}
