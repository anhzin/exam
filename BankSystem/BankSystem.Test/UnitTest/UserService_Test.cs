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
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BankSystem.Test.UnitTest
{
    public class UserService_Test : TestsBase
    {

        [Fact, TestPriority(0)]
        public void CanCreateUser()
        {

            var userID = CreateUserForTest();

            User foundEntity;
            using (var context = new BankSystemContext(options))
            {
                foundEntity = context.Users.FirstOrDefault(x => x.ID == userID);
            }

            Assert.NotNull(foundEntity);
        }

        [Fact, TestPriority(1)]
        public void CanGetUserById()
        {
            User foundEntity;
            var userID = CreateUserForTest();
            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userService = new UserService(uow);
                foundEntity = userService.GetUserById(userID);

            }
            Assert.NotNull(foundEntity);

        }

        [Fact, TestPriority(2)]
        public void CanGetUserByUserNameAndPassword()
        {
            User foundEntity;
            var userName = CreateUserForTestName();
            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userService = new UserService(uow);
                foundEntity = userService.GetUserByUserNameAndPassword(userName, "pwd");
            }

            Assert.NotNull(foundEntity);
        }

        [Fact, TestPriority(3)]
        public void CanGetUserByUserName()
        {
            User foundEntity;
            var userName = CreateUserForTestName();
            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userService = new UserService(uow);
                foundEntity = userService.GetUserByUserName(userName);
            }

            Assert.NotNull(foundEntity);
        }

        [Fact, TestPriority(4)]
        public void CanDepositeWithUserHasExistInSystem()
        {
            OperationStatus ops;
            var userID = CreateUserForTest();
            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userService = new UserService(uow);
                ops = userService.Deposite(userID, 1000);
            }

            Assert.True(ops.Status);
        }

        [Fact, TestPriority(5)]
        public void CanNotDepositeWithUserHasNotExistInSystem()
        {
            OperationStatus ops;

            var userID = Guid.NewGuid();
            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userService = new UserService(uow);
                ops = userService.Deposite(userID, 1000);
            }

            Assert.False(ops.Status);
        }

        [Fact, TestPriority(7)]
        public void CanWithDrawWithUserHasExistInSystem()
        {
            OperationStatus ops;
            var userID = CreateUserForTest();
            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userService = new UserService(uow);
                ops = userService.Deposite(userID, 1000);
                Assert.True(ops.Status);
                ops = userService.WithDraw(userID, 1000);
                Assert.True(ops.Status);

            }
        }

        [Fact, TestPriority(8)]
        public void CanNotWithDrawWithUserHasNotExistInSystem()
        {
            OperationStatus ops;

            var userID = Guid.NewGuid();
            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userService = new UserService(uow);
                ops = userService.WithDraw(userID, 1000);
                Assert.False(ops.Status);
            }

        }

        [Fact, TestPriority(9)]
        public void CanNotWithDrawIfAmountLargeThanBalance()
        {
            OperationStatus ops;
            User foundEntity;
            decimal userBalance;
            var userID = CreateUserForTest();
            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userService = new UserService(uow);
                foundEntity = userService.GetUserById(userID);
                Assert.NotNull(foundEntity);
                userBalance = foundEntity.Balance;
                ops = userService.WithDraw(userID, userBalance + 1000);
                Assert.False(ops.Status);
            }
        }


        [Fact, TestPriority(9)]
        public void CanTransferIfTwoUserExist()
        {
            OperationStatus ops;
            var userID_1 = CreateUserForTest();
            var userID_2 = CreateUserForTest();
            User user1, user2;
            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userService = new UserService(uow);
                ops = userService.Transfer(userID_1, 1000, userID_2);

                user1 = userService.GetUserById(userID_1);
                Assert.NotNull(user1.Balance);
                Assert.Equal(0, user1.Balance);

                user2 = userService.GetUserById(userID_2);
                Assert.NotNull(user2.Balance);
                Assert.Equal(2000, user2.Balance);
            }

            Assert.True(ops.Status);
        }

        [Fact, TestPriority(9)]
        public void CanNotTransferIfUserSourceNotExist()
        {
            OperationStatus ops;
            var userID_1 = Guid.NewGuid();
            var userID_2 = CreateUserForTest();
            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userService = new UserService(uow);
                ops = userService.Transfer(userID_1, 1000, userID_2);
            }

            Assert.False(ops.Status);
        }

        [Fact, TestPriority(9)]
        public void CanNotTransferIfUserTargetNotExist()
        {
            OperationStatus ops;
            var userID_1 = CreateUserForTest();
            var userID_2 = Guid.NewGuid();
            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userService = new UserService(uow);
                ops = userService.Transfer(userID_1, 1000, userID_2);
            }

            Assert.False(ops.Status);
        }

        [Fact, TestPriority(9)]
        public void CanNotTransferIfBalanceNotEnough()
        {
            OperationStatus ops;
            var userID_1 = CreateUserForTest();
            var userID_2 = CreateUserForTest();
            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userService = new UserService(uow);
                ops = userService.Transfer(userID_1, 2000, userID_2);
            }

            Assert.False(ops.Status);
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

        public string CreateUserForTestName()
        {
            Guid userID;
            Random rnd = new Random();
            int rndNumber = rnd.Next(1, 10000000);
            var userName = "user" + rndNumber;
            using (var context = new BankSystemContext(options))
            {
                userID = Guid.NewGuid();
                User exitEntityWithUserName = null;
                do
                {
                    exitEntityWithUserName = context.Users.FirstOrDefault(x => x.AccountName.Equals(userName));
                    if (exitEntityWithUserName != null)
                    {
                        rnd = new Random();
                        rndNumber = rnd.Next(1, 10000000);
                        userName = "user" + rndNumber;
                    }
                } while (exitEntityWithUserName != null);
            }

            var testEntity = new User
            {
                ID = userID,
                AccountNumber = Guid.NewGuid(),
                AccountName = userName,
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

            return userName;
        }
    }
}
