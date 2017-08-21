using BankSystem.BusinessLogic.Services;
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
    public class BankSystemContext_Test: MockContext
    {
        [Fact(DisplayName = "BankSystemContext_Should_Fetch_Database_Record")]
        public void RahmatSsoDbContext_Should_Fetch_Database_Record()
        {
            using (var ctx = new BankSystemContext())
            {
                var result = ctx.Users.First();
                Assert.NotNull(result);
            }
        }
    }
}
