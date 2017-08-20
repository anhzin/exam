using BankSystem.BusinessLogic.Services;
using BankSystem.Test.Controllers;
using BankSystem.Test.Data;
using BankSystem.Test.DataAccess.Repositories;
using BankSystem.Test.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BankSystem.Test.UnitTest
{
    public class UserController_Test : TestsBase
    {
        [Fact]
        public void ActionCreateShouldReturnIActionResult()
        {
            var controller = new UserController(bsc);
            var result = controller.Create() as IActionResult;
            Assert.NotNull(result);
        }

        [Fact]
        public void ActionCreateShouldRedirectToActionIndexWhenSuccess()
        {

            var user = CreateUserForTesting();
            using (var context = new BankSystemContext(options))
            {
                var controller = new UserController(context);
                var result = (RedirectToActionResult)controller.Create(user);
                Assert.Equal("Index", result.ActionName);
            }
        }

        [Fact]
        public void ActionCreateShouldReturnToViewCreateIfInputUserNotValid()
        {
            using (var context = new BankSystemContext(options))
            {
                var controller = new UserController(context);
                var result = (ViewResult)controller.Create(null);
                Assert.True(result.ViewData.ModelState["Create"].Errors.Count > 0);
            }
        }

        [Fact]
        public void ActionCreateShouldReturnToViewCreateIfUserExist()
        {
            var user = CreateUserForTesting();

            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userService = new UserService(uow);
                if (userService.Register(user).Status)
                {
                    var controller = new UserController(context);
                    var result = (ViewResult)controller.Create(user);
                    Assert.True(result.ViewData.ModelState["Create"].Errors.Count > 0);
                }
                else
                {
                    Assert.True(false);
                }

            }
        }

        [Fact]
        public void ActionLoginShouldReturnIActionResult()
        {
            var controller = new UserController(bsc);
            var result = controller.Login() as IActionResult;
            Assert.NotNull(result);
        }

        [Fact]
        public void ActionLoginShouldRedirectToActionDetailWhenSuccess()
        {
            var user = CreateUserForTesting();

            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userService = new UserService(uow);
                if (userService.Register(user).Status)
                {
                    var controller = new UserController(context);
                    var result = (RedirectToActionResult)controller.Login(user);
                    Assert.Equal("Details", result.ActionName);
                }
                else
                {
                    Assert.True(false);
                }

            }
        }


        [Fact]
        public void ActionLoginShouldReturnToViewLoginIfUserNotValid()
        {
            using (var context = new BankSystemContext(options))
            {
                var controller = new UserController(context);
                var result = (ViewResult)controller.Login(null);
                Assert.True(result.ViewData.ModelState["Login"].Errors.Count > 0);
            }
        }

        [Fact]
        public void ActionLoginShouldReturnToViewLoginIfUserNotExist()
        {
            using (var context = new BankSystemContext(options))
            {
                var controller = new UserController(context);
                var user = new User();
                var result = (ViewResult)controller.Login(user);
                Assert.True(result.ViewData.ModelState["Login"].Errors.Count > 0);

                user.AccountName = "asdasdasasd";
                user.Password = "assasdsadas";
                result = (ViewResult)controller.Login(user);
                Assert.True(result.ViewData.ModelState["Login"].Errors.Count > 0);
            }
        }

        [Fact]
        public void ActionDetailsShouldReturnToViewDetailsWhenSuccess()
        {
            var user = CreateUserForTesting();

            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userService = new UserService(uow);
                if (userService.Register(user).Status)
                {
                    var controller = new UserController(context);
                    var result = controller.Details(user.ID) as ViewResult;
                    Assert.NotNull(result);
                    Assert.Equal("Details", result.ViewName);
                }
                else
                {
                    Assert.True(false);
                }

            }
        }

        [Fact]
        public void ActionDetailsShouldRedirectToHomeIfUserIdNotValid()
        {
            using (var context = new BankSystemContext(options))
            {
                var controller = new UserController(context);
                var result = (RedirectToActionResult)controller.Details(Guid.NewGuid());
                Assert.Equal("Index", result.ActionName);
                Assert.Equal("Home", result.ControllerName);
            }
        }

        [Fact]
        public void ActionDepositeShouldReturnToViewDepositeIfUserExist()
        {
            var user = CreateUserForTesting();

            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userService = new UserService(uow);
                if (userService.Register(user).Status)
                {
                    var controller = new UserController(context);
                    var result = controller.Deposite(user.ID) as ViewResult;
                    Assert.NotNull(result);
                }
                else
                {
                    Assert.True(false);
                }

            }
        }
        
        [Fact]
        public void ActionDepositeShouldRedirectToHomeIfUserIdNotValid()
        {
            using (var context = new BankSystemContext(options))
            {
                var controller = new UserController(context);
                var result = (RedirectToActionResult)controller.Deposite(Guid.NewGuid());
                Assert.Equal("Index", result.ActionName);
                Assert.Equal("Home", result.ControllerName);
            }
        }

        [Fact]
        public void ActionWithDrawShouldReturnToViewWithDrawIfUserExist()
        {
            var user = CreateUserForTesting();

            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userService = new UserService(uow);
                if (userService.Register(user).Status)
                {
                    var controller = new UserController(context);
                    var result = controller.WithDraw(user.ID) as ViewResult;
                    Assert.NotNull(result);
                }
                else
                {
                    Assert.True(false);
                }

            }
        }

        [Fact]
        public void ActionWithDrawShouldReturnToViewWithDrawWhenSuccess()
        {
            var user = CreateUserForTesting();

            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userService = new UserService(uow);
                if (userService.Register(user).Status)
                {
                    var controller = new UserController(context);
                    var result = controller.WithDraw(user.ID) as ViewResult;
                    Assert.NotNull(result);
                    var result1 = controller.WithDraw(300) as RedirectToActionResult;
                    Assert.NotNull(result1);
                    Assert.Equal("Details", result1.ActionName);
                }
                else
                {
                    Assert.True(false);
                }

            }
        }

        [Fact]
        public void ActionWithDrawShouldReturnToViewWithDrawWhenAmountNumberLargerThanBalance()
        {
            var user = CreateUserForTesting();

            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userService = new UserService(uow);
                if (userService.Register(user).Status)
                {
                    var controller = new UserController(context);
                    var result = controller.WithDraw(user.ID) as ViewResult;
                    Assert.NotNull(result);
                    result = controller.WithDraw(2000) as ViewResult;
                    Assert.NotNull(result);
                    Assert.True(result.ViewData.ModelState["WithDraw"].Errors.Count > 0);
                }
                else
                {
                    Assert.True(false);
                }

            }
        }

        [Fact]
        public void ActionWithDrawShouldRedirectToDetailsWhenSuccess()
        {
            var user = CreateUserForTesting();

            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userService = new UserService(uow);
                if (userService.Register(user).Status)
                {
                    var controller = new UserController(context);
                    var result = controller.WithDraw(user.ID) as ViewResult;
                    Assert.NotNull(result);
                    var result1 = controller.WithDraw(100) as RedirectToActionResult;
                    Assert.Equal("Details", result1.ActionName);
                }
                else
                {
                    Assert.True(false);
                }
            }
        }

        [Fact]
        public void ActionWithDrawShouldRedirectToHomeIfUserIdNotValid()
        {
            using (var context = new BankSystemContext(options))
            {
                var controller = new UserController(context);
                var result = (RedirectToActionResult)controller.WithDraw(Guid.NewGuid());
                Assert.Equal("Index", result.ActionName);
                Assert.Equal("Home", result.ControllerName);
            }
        }

        [Fact]
        public void ActionTransferShouldReturnToViewTransferIfUserExist()
        {
            var user = CreateUserForTesting();

            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userService = new UserService(uow);
                if (userService.Register(user).Status)
                {
                    var controller = new UserController(context);
                    var result = controller.Transfer(user.ID) as ViewResult;
                    Assert.NotNull(result);
                }
                else
                {
                    Assert.True(false);
                }

            }
        }

        [Fact]
        public void ActionTransferShouldReturnToViewTransferIfUserTargetNotExist()
        {
            var user = CreateUserForTesting();

            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userService = new UserService(uow);
                if (userService.Register(user).Status)
                {
                    var controller = new UserController(context);
                    var result = controller.Transfer(user.ID) as ViewResult;
                    Assert.NotNull(result);

                    var result1 = controller.Transfer(100, Guid.NewGuid()) as ViewResult;
                    Assert.NotNull(result1);
                    Assert.True(result1.ViewData.ModelState["Transfer"].Errors.Count > 0);
                }
                else
                {
                    Assert.True(false);
                }

            }
        }

        [Fact]
        public void ActionTransferShouldReturnToViewTransferIfAmountNumberLargerThanBalance()
        {
            var user = CreateUserForTesting();
            var user2 = CreateUserForTesting();
            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userService = new UserService(uow);
                if (userService.Register(user).Status && userService.Register(user2).Status)
                {
                    var controller = new UserController(context);
                    var result = controller.Transfer(user.ID) as ViewResult;
                    Assert.NotNull(result);

                    var result1 = controller.Transfer(3000, user2.ID) as ViewResult;
                    Assert.NotNull(result1);
                    Assert.True(result1.ViewData.ModelState["Transfer"].Errors.Count > 0);
                }
                else
                {
                    Assert.True(false);
                }

            }
        }

        [Fact]
        public void ActionTransferShouldReturnToViewTransferWhenSuccess()
        {
            var user = CreateUserForTesting();
            var user2 = CreateUserForTesting();
            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userService = new UserService(uow);
                if (userService.Register(user).Status && userService.Register(user2).Status)
                {
                    var controller = new UserController(context);
                    var result = controller.Transfer(user.ID) as ViewResult;
                    Assert.NotNull(result);

                    var result1 = controller.Transfer(5, user2.ID) as RedirectToActionResult;
                    Assert.NotNull(result1);
                    Assert.Equal("Details", result1.ActionName);
                }
                else
                {
                    Assert.True(false);
                }

            }
        }


        [Fact]
        public void ActionTransferShouldRedirectToHomeIfUserIdNotValid()
        {
            using (var context = new BankSystemContext(options))
            {
                var controller = new UserController(context);
                var result = (RedirectToActionResult)controller.Transfer(Guid.NewGuid());
                Assert.Equal("Index", result.ActionName);
                Assert.Equal("Home", result.ControllerName);
            }
        }

        [Fact]
        public void ActionReportShouldRedirectToHomeIfUserIdNotValid()
        {
            using (var context = new BankSystemContext(options))
            {
                var controller = new UserController(context);
                var result = (RedirectToActionResult)controller.Report(Guid.NewGuid());
                Assert.Equal("Index", result.ActionName);
                Assert.Equal("Home", result.ControllerName);
            }
        }

        [Fact]
        public void ActionReportShouldRedirectToHomeIfUserExist()
        {
            var user = CreateUserForTesting();

            using (var context = new BankSystemContext(options))
            {
                var uow = new UnitOfWork(context);
                var userService = new UserService(uow);
                if (userService.Register(user).Status)
                {
                    var controller = new UserController(context);
                    var result = controller.Report(user.ID) as ViewResult;
                    Assert.NotNull(result);
                }
                else
                {
                    Assert.True(false);
                }

            }
        }

        public User CreateUserForTesting()
        {
            //  
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

            var user = new User
            {
                ID = userID,
                AccountNumber = Guid.NewGuid(),
                AccountName = UserName,
                Balance = 1000,
                Password = "pwd",
                CreatedDate = DateTime.Now
            };

            return user;
        }
    }
}
