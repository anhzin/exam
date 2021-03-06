﻿
using BankSystem.Test.DataAccess.Repositories;
using BankSystem.Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankSystem.BusinessLogic.Services
{
    public class TransactionService 
    {
        UnitOfWork _unitOfWork;
        public TransactionService(UnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public void AddNewTransaction(Transaction transaction)
        {
            _unitOfWork.TransactionRepository.Add(transaction);
            _unitOfWork.TransactionRepository.SaveChanges();
        }

        public IEnumerable<Transaction> GetAllTransactionOfUser(Guid id)
        {
            return _unitOfWork.TransactionRepository.Find(n => n.UserID == id).OrderByDescending(t => t.CreatedDate);
        }
    }
}
