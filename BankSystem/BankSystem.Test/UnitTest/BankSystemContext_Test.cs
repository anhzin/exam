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
    public class BankSystemContext_Test
    {
        private readonly DbContextOptions<BankSystemContext> options;

        public BankSystemContext_Test()
        {
            var connectionString = @"Server=(localdb)\mssqllocaldb;Database=BankSystem_Test;Integrated Security=True;Trusted_Connection=True;MultipleActiveResultSets=true";

            options = new DbContextOptionsBuilder<BankSystemContext>()
                   .UseSqlServer(connectionString)
                   .Options;


            var services = new ServiceCollection()
     .AddDbContext<BankSystemContext>(
         b => b.UseSqlServer(connectionString));
            // Create the schema in the database
            using (var bsc = new BankSystemContext(options))
            {

                bsc.Database.EnsureCreatedAsync();
            }
        }

        [Fact]
        public void CanCreateUser()
        {

            
        }
    }
}
