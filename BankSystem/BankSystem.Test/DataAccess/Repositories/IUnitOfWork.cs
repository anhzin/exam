using BankSystem.Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankSystem.Test.DataAccess.Repositories
{
    public interface IUnitOfWork
    {
        IRepository<Transaction> TransactionRepository { get; }
        IRepository<User> UserRepository { get; }

        /// <summary>
        /// Commits all changes
        /// </summary>
        void Commit();
        /// <summary>
        /// Discards all changes that has not been commited
        /// </summary>
        void RejectChanges();
        void Dispose();
    }
}
