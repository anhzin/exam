using BankSystem.Data;
using BankSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankSystem.Services
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private BankSystemContext context;

        public UserRepository(BankSystemContext context)
        {
            this.context = context;
        }
        
        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<User> GetUsers()
        {
            return context.Users.ToList();
        }

        public User GetUserByID(Guid userId)
        {
            return context.Users.Find(userId);
        }

        public void InsertUser(User user)
        {
            context.Users.Add(user);
        }

        public void DeleteUser(Guid userID)
        {
            User user = context.Users.Find(userID);
            context.Users.Remove(user);
        }

        public void UpdateUser(User user)
        {
            context.Entry(user).State = EntityState.Modified;
        }

        public User GetUserByUserNameAndPassword(string userName, string password)
        {
            return context.Users
                .SingleOrDefault(m => m.AccountName.Equals(userName) && m.Password.Equals(password));
        }

        public int Deposite(decimal amount)
        {
            throw new NotImplementedException();
        }

        public int WithDraw(decimal amount)
        {
            throw new NotImplementedException();
        }

        public int Transfer(decimal amount, string target)
        {
            throw new NotImplementedException();
        }
    }
}
