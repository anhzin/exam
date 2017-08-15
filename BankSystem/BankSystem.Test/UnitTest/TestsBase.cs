using BankSystem.Test.Data;
using BankSystem.Test.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BankSystem.Test.UnitTest
{
    public abstract class TestsBase : IDisposable
    {
        public readonly DbContextOptions<BankSystemContext> options;
        public readonly BankSystemContext bsc;
        public Guid UserId = Guid.Parse("ffb27027-eec4-4952-99c5-14e8b5185cc8");
        public String UserName = "user1";
        protected TestsBase()
        {
            var connectionString = @"Server=(localdb)\mssqllocaldb;Database=BankSystem_Test;Integrated Security=True;Trusted_Connection=True;MultipleActiveResultSets=true";

            options = new DbContextOptionsBuilder<BankSystemContext>()
                   .UseSqlServer(connectionString)
                   .Options;


            var services = new ServiceCollection()
     .AddDbContext<BankSystemContext>(
         b => b.UseSqlServer(connectionString));
            // Create the schema in the database
            using (bsc = new BankSystemContext(options))
            {
               bsc.Database.EnsureCreatedAsync();
                Thread.Sleep(1500); //wait for system create
                if (bsc.Users == null|| bsc.Users.Any())
                {
                    return;   // DB has been seeded
                }
               
                Guid userID;
                Random rnd = new Random();
                int rndNumber = rnd.Next(1, 10000000);
                var userName = "user" + rndNumber;
                var user = new User
                {
                    ID = userID,
                    AccountNumber = Guid.NewGuid(),
                    AccountName = userName,
                    Balance = 1000,
                    Password = "pwd",
                    CreatedDate = DateTime.Now
                };
                bsc.Users.Add(user);

                if (bsc.Transactions.Any())
                {
                    return;   // DB has been seeded
                }
                var transaction = new Transaction()
                {
                    ID = Guid.NewGuid(),
                    Type = TransactionTypes.Deposite,
                    CreatedDate = DateTime.Now,
                    Status = true,
                    Amount = 1000,
                    UserID = userID,
                    Target = null
                };
                bsc.Transactions.Add(transaction);
                bsc.SaveChanges();
            }
        }


        public void Dispose()
        {
          //  bsc.Dispose();
        }

    }

}
