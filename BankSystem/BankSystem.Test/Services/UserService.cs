
using BankSystem.Common;
using BankSystem.Test.DataAccess.Repositories;
using BankSystem.Test.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankSystem.BusinessLogic.Services
{
    public class UserService
    {
        UnitOfWork _unitOfWork;
        public UserService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void Register(User user)
        {
            _unitOfWork.UserRepository.Add(user);
            _unitOfWork.UserRepository.SaveChanges();
        }
        public User GetUserById(Guid id)
        {
            return _unitOfWork.UserRepository.Find(n => n.ID == id).SingleOrDefault();
        }

        public User GetUserByUserNameAndPassword(string userName, string password)
        {
            return _unitOfWork.UserRepository.Find(m => m.AccountName.Equals(userName) && m.Password.Equals(password)).SingleOrDefault();
        }
        public User GetUserByUserName(string userName)
        {
            return _unitOfWork.UserRepository.Find(m => m.AccountName.Equals(userName)).SingleOrDefault();
        }

        public OperationStatus Deposite(Guid id, decimal amount)
        {
            var opStatus = new OperationStatus { Status = true };
            var user = GetUserById(id);
            if (user == null)
            {
                opStatus.Status = false;
                opStatus.ExceptionMessage = "Unable to deposite. Your account is not exist!";
            }
            else
            {
                try
                {
                    user.Balance += amount;
                    _unitOfWork.UserRepository.Update(user);
                    _unitOfWork.UserRepository.SaveChanges();

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (User)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        opStatus.Status = false;
                        opStatus.ExceptionMessage = "Unable to deposite. Your account is not exist!";
                    }
                    else
                    {
                        var databaseValues = (User)databaseEntry.ToObject();
                        user.Balance = databaseValues.Balance + amount;
                        _unitOfWork.UserRepository.Update(user);

                        _unitOfWork.UserRepository.SaveChanges();
                    }

                }
            }

            return opStatus;
        }

        public OperationStatus WithDraw(Guid id, decimal amount)
        {
            var opStatus = new OperationStatus { Status = true };
            var user = GetUserById(id);
            if (user == null)
            {
                opStatus.Status = false;
                opStatus.ExceptionMessage = "Unable to WithDraw. Your account is not exist!";
            }
            else
            {
                try
                {
                    if (user.Balance >= amount)
                    {
                        try
                        {
                            user.Balance -= amount;
                            _unitOfWork.UserRepository.Update(user);

                            _unitOfWork.UserRepository.SaveChanges();

                        }
                        catch (DbUpdateConcurrencyException ex)
                        {
                            var entry = ex.Entries.Single();
                            var clientValues = (User)entry.Entity;
                            var databaseEntry = entry.GetDatabaseValues();
                            if (databaseEntry == null)
                            {
                                opStatus.Status = false;
                                opStatus.ExceptionMessage = "Unable to WithDraw. Your account is not exist!";
                            }
                            else
                            {
                                var databaseValues = (User)databaseEntry.ToObject();
                                if (databaseValues.Balance >= amount)
                                {
                                    user.Balance = databaseValues.Balance - amount;
                                    _unitOfWork.UserRepository.Update(user);

                                    _unitOfWork.UserRepository.SaveChanges();
                                }
                                else
                                {
                                    opStatus.Status = false;
                                    opStatus.ExceptionMessage = "Unable to WithDraw. Your account is not enough money!";
                                }

                            }

                        }

                    }
                    else
                    {
                        opStatus.Status = false;
                        opStatus.ExceptionMessage = "Unable to WithDraw. Your account is not enough money!";
                    }

                }
                catch (Exception ex)
                {
                    opStatus.Status = false;
                    opStatus.ExceptionMessage = "Unable to WithDraw. Try again, and if the problem persists contact your system administrator.";

                }
            }

            return opStatus;
        }

        public OperationStatus Transfer(Guid id, decimal amount, Guid idTarget)
        {
            var opStatus = new OperationStatus { Status = true };
            var user = GetUserById(id);
            var userTarget = GetUserById(idTarget);
            if (user == null)
            {

                opStatus.Status = false;
                opStatus.ExceptionMessage = "Unable to Transfer. Your account is not exist!";
            }
            else if (userTarget == null)
            {
                opStatus.Status = false;
                opStatus.ExceptionMessage = "Unable to Transfer. Cannot find the target account!";
            }
            else
            {
                try
                {
                    if (user.Balance >= amount)
                    {
                        try
                        {
                            user.Balance -= amount;
                            _unitOfWork.UserRepository.Update(user);

                            _unitOfWork.UserRepository.SaveChanges();
                        }
                        catch (DbUpdateConcurrencyException ex)
                        {
                            var entry = ex.Entries.Single();
                            var clientValues = (User)entry.Entity;
                            var databaseEntry = entry.GetDatabaseValues();
                            if (databaseEntry == null)
                            {
                                opStatus.Status = false;
                                opStatus.ExceptionMessage = "Unable to Transfer. Your account is not exist!";
                            }
                            else
                            {
                                var databaseValues = (User)databaseEntry.ToObject();
                                if (databaseValues.Balance > amount)
                                {
                                    user.Balance = databaseValues.Balance - amount;
                                    _unitOfWork.UserRepository.Update(user);

                                    _unitOfWork.UserRepository.SaveChanges();
                                }
                                else
                                {
                                    opStatus.Status = false;
                                    opStatus.ExceptionMessage = "Unable to WithDraw. Your account is not enough money!";
                                }

                            }

                        }
                        try
                        {
                            userTarget.Balance += amount;
                            _unitOfWork.UserRepository.Update(userTarget);

                            _unitOfWork.UserRepository.SaveChanges();

                        }
                        catch (DbUpdateConcurrencyException ex)
                        {
                            var entry = ex.Entries.Single();
                            var clientValues = (User)entry.Entity;
                            var databaseValues = GetUserById(idTarget);
                            if (databaseValues == null)
                            {
                                user.Balance += amount; // refund
                                _unitOfWork.UserRepository.Update(user);

                                _unitOfWork.UserRepository.SaveChanges();
                                opStatus.Status = false;
                                opStatus.ExceptionMessage = "Unable to Transfer. Cannot find the target account!";

                            }
                            else
                            {
                                userTarget.Balance = databaseValues.Balance + amount;
                                _unitOfWork.UserRepository.Update(user);

                            }
                        }
                    }
                    else
                    {
                        opStatus.Status = false;
                        opStatus.ExceptionMessage = "Unable to WithDraw. Your account is not enough money!";

                    }

                }
                catch (Exception ex)
                {
                    opStatus.Status = false;
                    opStatus.ExceptionMessage = "Unable to Transfer. Try again, and if the problem persists contact your system administrator.";
                }
            }


            return opStatus;
        }
    }
}
