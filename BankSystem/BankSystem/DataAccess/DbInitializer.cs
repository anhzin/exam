using BankSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankSystem.Data
{
    public class DbInitializer
    {
        public static void Initialize(BankSystemContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }
            var users = new List<User>();
            for (int i = 0; i < 10; i++)
            {
                var user =
                new User
                {
                    ID = Guid.NewGuid(),
                    AccountNumber = Guid.NewGuid(),
                    AccountName = "user " + i,
                    Balance = 1000 * i,
                    Password = "pwd"+i,
                    CreatedDate = DateTime.Now
                };
                users.Add(user);
            }

            context.Users.AddRange(users);
            context.SaveChanges();
            

        }
    }
}

