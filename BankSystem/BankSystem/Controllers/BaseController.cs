using BankSystem.BusinessLogic.Services;
using BankSystem.Data;
using BankSystem.DataAccess;
using BankSystem.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankSystem.Controllers
{
    public class BaseController : Controller
    {
        private readonly BankSystemContext _context;
        private static UserService _userService;
        private static UnitOfWork _unitOfwork;

        public BaseController()
        {
            _unitOfwork = new UnitOfWork(_context);
        }
    }
}
