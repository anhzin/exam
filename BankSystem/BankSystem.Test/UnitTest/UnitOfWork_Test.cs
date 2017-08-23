using BankSystem.Test.Controllers;
using BankSystem.Test.Data;
using BankSystem.Test.DataAccess.Repositories;
using BankSystem.Test.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BankSystem.Test.UnitTest
{
    public class UnitOfWork_Test : TestsBase
    {
        [Fact(DisplayName = "CanGetAllUser")]
        public void CanGetAllUser()
        {
            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var users = uow.UserRepository.GetAll();
                Assert.True(users.Count() > 1);
            }

        }

        [Fact(DisplayName = "CanAddUser")]
        public void CanAddUser()
        {
            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userId = Guid.NewGuid();
                var user = new User()
                {
                    ID = userId,
                    AccountName = "Zin"
                };
                var isAdded = uow.UserRepository.Add(user);
                Assert.True(isAdded);
            }
        }

        [Fact(DisplayName = "CanDeleteUser")]
        public void CanDeleteUser()
        {
            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userId = Guid.NewGuid();
                var user = new User()
                {
                    ID = userId,
                    AccountName = "Zin"
                };
                var isAdded = uow.UserRepository.Add(user);
                Assert.True(isAdded);

                var isDeleted = uow.UserRepository.Delete(user);
                Assert.True(isDeleted);
            }
        }

        [Fact(DisplayName = "CanUpdateUser")]
        public void CanUpdateUser()
        {
            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userId = Guid.NewGuid();
                var user = new User()
                {
                    ID = userId,
                    AccountName = "Zin"
                };
                var isAdded = uow.UserRepository.Add(user);
                Assert.True(isAdded);

                user.AccountName = "Nguyen";
                var isUpdated = uow.UserRepository.Update(user);
                Assert.True(isUpdated);

                var userActual = uow.UserRepository.GetById(user.ID);
                Assert.NotNull(userActual);
                Assert.True(userActual.AccountName.Equals("Nguyen"));
            }
        }

        [Fact(DisplayName = "CanGetDetailUser")]
        public void CanGetDetailUser()
        {
            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userId = Guid.NewGuid();
                var user = new User()
                {
                    ID = userId,
                    AccountName = "Zin"
                };
                var isAdded = uow.UserRepository.Add(user);
                Assert.True(isAdded);

                var userActual = uow.UserRepository.GetById(user.ID);
                Assert.NotNull(userActual);
                Assert.True(userActual.AccountName.Equals("Zin"));
            }
        }

        [Fact(DisplayName = "CanAddTransaction")]
        public void CanAddTransaction()
        {
            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var transactionId = Guid.NewGuid();
                var userId = Guid.NewGuid();
                var transaction = new Transaction()
                {
                    ID = transactionId,
                    Type = TransactionTypes.Deposite,
                    Amount = 100,
                    Status = true,
                    CreatedDate = DateTime.Now,
                    UserID = userId
                };

                var isAdded = uow.TransactionRepository.Add(transaction);
                Assert.True(isAdded);
            }
        }

        [Fact(DisplayName = "CanGetAllTransaction")]
        public void CanGetAllTransaction()
        {
            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var transactionId = Guid.NewGuid();
                var userId = Guid.NewGuid();
                var transaction = new Transaction()
                {
                    ID = transactionId,
                    Type = TransactionTypes.Deposite,
                    Amount = 100,
                    Status = true,
                    CreatedDate = DateTime.Now,
                    UserID = userId
                };

                var isAdded = uow.TransactionRepository.Add(transaction);
                Assert.True(isAdded);

                var transactions = uow.TransactionRepository.GetAll();
                Assert.True(transactions.Count()>0);
            }
        }

        [Fact(DisplayName = "CanGetDetailTransaction")]
        public void CanGetDetailTransaction()
        {
            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var transactionId = Guid.NewGuid();
                var userId = Guid.NewGuid();
                var transaction = new Transaction()
                {
                    ID = transactionId,
                    Type = TransactionTypes.Deposite,
                    Amount = 100,
                    Status = true,
                    CreatedDate = DateTime.Now,
                    UserID = userId
                };

                var isAdded = uow.TransactionRepository.Add(transaction);
                Assert.True(isAdded);

                var transactionActual = uow.TransactionRepository.GetById(transactionId);
                Assert.NotNull(transactionActual);
            }
        }
    }
}
