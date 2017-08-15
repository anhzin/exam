using BankSystem.BusinessLogic.Services;
using BankSystem.Common;
using BankSystem.Test.Data;
using BankSystem.Test.DataAccess.Repositories;
using BankSystem.Test.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BankSystem.Test.UnitTest
{
    public class TransactionService_Test : TestsBase
    {

        [Fact]
        public void CanAddNewTransaction()
        {
            OperationStatus ops;
            var userID = CreateUserForTest();
            var transactionID = Guid.NewGuid();
            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userService = new UserService(uow);
                ops = userService.Deposite(userID, 1000);
                Assert.True(ops.Status);
                var transactionService = new TransactionService(uow);
            
                var transaction = new Transaction()
                {
                    ID = transactionID,
                    Type = TransactionTypes.Deposite,
                    CreatedDate = DateTime.Now,
                    Status = ops.Status,
                    Amount = 1000,
                    UserID = userID,
                    Target = null
                };
                transactionService.AddNewTransaction(transaction);
                
               
            }

            Transaction foundEntity;
            using (var context = new BankSystemContext(options))
            {
                foundEntity =  context.Transactions.FirstOrDefault(x => x.ID == transactionID);
            }

            Assert.NotNull(foundEntity);
        }


        [Fact]
        public void CanGetAllTransactionOfUser()
        {
            OperationStatus ops;
            var userID = CreateUserForTest();
            var transactionID = Guid.NewGuid();
            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userService = new UserService(uow);
                ops = userService.Deposite(userID, 1000);
                Assert.True(ops.Status);
                var transactionService = new TransactionService(uow);

                var transaction = new Transaction()
                {
                    ID = transactionID,
                    Type = TransactionTypes.Deposite,
                    CreatedDate = DateTime.Now,
                    Status = ops.Status,
                    Amount = 1000,
                    UserID = userID,
                    Target = null
                };
                transactionService.AddNewTransaction(transaction);


            }

            Transaction foundEntity;
            using (var context = new BankSystemContext(options))
            {
                foundEntity = context.Transactions.FirstOrDefault(x => x.ID == transactionID);
            }

            Assert.NotNull(foundEntity);

            IEnumerable<Transaction> transactions;
            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var transactionService = new TransactionService(uow);
                transactions = transactionService.GetAllTransactionOfUser(userID);
            }

            Assert.Equal(1, transactions.Count());

        }
        public Guid CreateUserForTest()
        {
            Guid userID;
            Random rnd = new Random();
            int rndNumber = rnd.Next(1, 10000000);
            UserName = "user" + rndNumber;
            using (var context = new BankSystemContext(options))
            {
                userID = Guid.NewGuid();
                User exitEntityWithUserName = null;
                do
                {
                    exitEntityWithUserName = context.Users.FirstOrDefault(x => x.AccountName.Equals(UserName));
                    if (exitEntityWithUserName != null)
                    {
                        rnd = new Random();
                        rndNumber = rnd.Next(1, 10000000);
                        UserName = "user" + rndNumber;
                    }
                } while (exitEntityWithUserName != null);
            }

            var testEntity = new User
            {
                ID = userID,
                AccountNumber = Guid.NewGuid(),
                AccountName = UserName,
                Balance = 1000,
                Password = "pwd",
                CreatedDate = DateTime.Now
            };

            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userService = new UserService(uow);
                userService.Register(testEntity);
            }

            return userID;
        }
    }
}
