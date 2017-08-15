using BankSystem.Test.Controllers;
using BankSystem.Test.Data;
using BankSystem.Test.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
        public void CreateShouldReturnIActionResult()
        {
            var controller = new UserController(bsc);
            var result = controller.Create() as IActionResult;
            Assert.NotNull(result);
        }
        [Fact]
        public void LoginShouldReturnIActionResult()
        {
            var controller = new UserController(bsc);
            var result = controller.Login() as IActionResult;
            Assert.NotNull(result);
        }


    }
}
