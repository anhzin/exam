using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankSystem.Models;
using BankSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Services
{
    public class TransactionRepository : ITransactionRepository, IDisposable
    {
        private BankSystemContext context;

        public TransactionRepository(BankSystemContext context)
        {
            this.context = context;
        }
        
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Transaction GetTransactionByID(Guid transactiondID)
        {
            return context.Transactions.Find(transactiondID);
        }

        public IEnumerable<Transaction> GetTransactions()
        {
            return context.Transactions.ToList();
        }

        public void InsertTransaction(Transaction transaction)
        {
            context.Transactions.Add(transaction);
        }

        public void Save()
        {
            context.SaveChanges();
        }


        public void DeleteTransaction(Guid transactiondID)
        {
            Transaction transaction = context.Transactions.Find(transactiondID);
            context.Transactions.Remove(transaction);
        }

        public void UpdateTransaction(Transaction transaction)
        {
            context.Entry(transaction).State = EntityState.Modified;
        }
    }
}
