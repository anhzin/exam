using BankSystem.Test.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BankSystem.Test.UnitTest
{
    public class HomeController_Test
    {
        [Fact]
        public void IndexShouldReturnIActionResult()
        {
            var controller = new HomeController();
            var result = controller.Index() as IActionResult;
            Assert.NotNull(result);
        }

        [Fact]
        public void LoginShouldReturnIActionResult()
        {
            var controller = new HomeController();
            var result = controller.Login() as IActionResult;
            Assert.NotNull(result);
        }

        [Fact]
        public void RegisterShouldReturnIActionResult()
        {
            var controller = new HomeController();
            var result = controller.Register() as IActionResult;
            Assert.NotNull(result);
        }

        [Fact]
        public void ErrorShouldReturnIActionResult()
        {
            var controller = new HomeController();
            var result = controller.Error() as IActionResult;
            Assert.NotNull(result);
        }
    }
}
