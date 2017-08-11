using BankSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankSystem.Services
{
    public interface IUserRepository : IDisposable
    {
        IEnumerable<User> GetUsers();
        User GetUserByID(Guid userId);
        User GetUserByUserNameAndPassword(string userName, string password);
        void InsertUser(User user);
        void DeleteUser(Guid userID);
        void UpdateUser(User user);
        int Deposite(decimal amount);
        int WithDraw(decimal amount);
        int Transfer(decimal amount,string target);
        void Save();
        
    }
}
