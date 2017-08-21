
using BankSystem.Test.DataAccess.Repositories;
using BankSystem.Test.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BankSystem.Test.UnitTest
{
    public class IRepository_Test
    {
        private Mock<IRepository<User>> _userRepository;

        [Fact(DisplayName = "CanGetAllUser")]
        public void CanGetAllUser()
        {
            _userRepository = new Mock<IRepository<User>>();
            var userList = new List<User>();
            userList.Add(new User());
            _userRepository.Setup(m => m.GetAll()).Returns(userList);
            IList<User> users = _userRepository.Object.GetAll().ToList();

            Assert.True(users.Count >= 1);
        }

        [Fact(DisplayName = "CanGetDetailUser")]
        public void CanGetDetailUser()
        {
            _userRepository = new Mock<IRepository<User>>();
            var userId = Guid.NewGuid();
            var user = new User()
            {
                ID = userId,
                AccountName = "Zin"
            };
            _userRepository.Setup(m => m.GetById(userId)).Returns(user);
            var actualUser = _userRepository.Object.GetById(userId);

            Assert.NotNull(actualUser);
            Assert.True(actualUser.AccountName.Equals("Zin"));
            Assert.True(actualUser.ID == userId);
        }
    }
}
