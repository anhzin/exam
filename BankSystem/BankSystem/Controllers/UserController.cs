using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BankSystem.Data;
using BankSystem.Models;
using BankSystem.DataAccess;
using BankSystem.BusinessLogic.Services;
using BankSystem.DataAccess.Repositories;

namespace BankSystem.Controllers
{
    public class UserController: Controller
    {
        private readonly BankSystemContext _context;
        private static UserService _userService;
        private static TransactionService _transactionService;
        private static UnitOfWork _unitOfwork;

        
        private static Guid userID;

        public UserController(BankSystemContext context)
        {
            _unitOfwork = new UnitOfWork(context);
            _userService = new UserService(_unitOfwork);
            _transactionService = new TransactionService(_unitOfwork);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            userID = id;

            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,AccountName,Password")] User user)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    user.ID = Guid.NewGuid();
                    user.AccountNumber = Guid.NewGuid();
                    user.Balance = 0;
                    user.CreatedDate = DateTime.Now;
                    _userService.Register(user);

                    return RedirectToAction("Index", "Transactions", user.AccountNumber);
                }
            }
            catch (Exception ex)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
                return View(user);
            }
            return View(user);
        }


        // GET: Users/Login
        public IActionResult Login()
        {
            return View();
        }
        // POST: Users/Login
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login([Bind("AccountName,Password")] User user)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var user1 = _userService.GetUserByUserNameAndPassword(user.AccountName, user.Password);
                    if (user1 == null)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to login. Try again, and if the problem persists contact your system administrator.");
                        return View(user);
                    }

                    return RedirectToAction("Details", "User", new { id = user1.ID });

                }
                ModelState.AddModelError(string.Empty, "Unable to login. Try again, and if the problem persists contact your system administrator.");
                return View(user);
            }
            catch (Exception ex)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to login. Try again, and if the problem persists contact your system administrator.");
                return View(user);
            }
            // return RedirectToAction("Index", "Home", null);
        }

        
        // GET: Transactions/Deposite
        public IActionResult Deposite(Guid id)

        {
            userID = id;
            return View();
        }

        // POST: Transactions/Deposite
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deposite(decimal amount)
        {
            var transaction = new Transaction();
            if (ModelState.IsValid)
            {
                var operationStatus = _userService.Deposite(userID, amount);
                bool statusTrans = false;
                if (!operationStatus.Status)
                {
                    statusTrans = true;
                    ModelState.AddModelError(string.Empty, operationStatus.ExceptionMessage);
                    return View(transaction);
                }
                CreateAndAddTransaction(TransactionTypes.Deposite, amount, statusTrans, userID, string.Empty);
                return RedirectToAction("Details", new { id = userID });
            }

            return View(transaction);
        }

        // GET: Transactions/WithDraw
        public IActionResult WithDraw(Guid id)
        {
            userID = id;
            return View();
        }

        // POST: Transactions/WithDraw
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WithDraw(decimal amount)
        {
            var transaction = new Transaction();
            if (ModelState.IsValid)
            {
                var operationStatus = _userService.Deposite(userID, amount);
                bool statusTrans = false;
                if (!operationStatus.Status)
                {
                    statusTrans = true;
                    ModelState.AddModelError(string.Empty, operationStatus.ExceptionMessage);
                    return View(transaction);
                }
                CreateAndAddTransaction(TransactionTypes.Deposite, amount, statusTrans, userID, string.Empty);
                return RedirectToAction("Details", new { id = userID });
            }

            return View(transaction);
        }

        // GET: Transactions/Transfer
        public IActionResult Transfer(Guid id)
        {
            userID = id;
            return View();
        }

        // POST: Transactions/WithDraw
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Transfer(decimal amount, Guid target)
        {
            var transaction = new Transaction();
            if (ModelState.IsValid)
            {
                var operationStatus = _userService.Transfer(userID, amount,target);
                bool statusTrans = false;
                if (!operationStatus.Status)
                {
                    statusTrans = true;
                    ModelState.AddModelError(string.Empty, operationStatus.ExceptionMessage);
                    return View(transaction);
                }
                CreateAndAddTransaction(TransactionTypes.Deposite, amount, statusTrans, userID, string.Empty);
                return RedirectToAction("Details", new { id = userID });
            }

            return View(transaction);
        }
        // GET: Transactions
        public async Task<IActionResult> Report(Guid id)
        {
            var listTransactions = _transactionService.GetAllTransactionOfUser(id);
            return View(listTransactions);
        }
        

        private void CreateAndAddTransaction(TransactionTypes type, decimal amount, bool status, Guid userID, string target)
        {
            var transaction = new Transaction()
            {
                ID = Guid.NewGuid(),
                Type = type,
                CreatedDate = DateTime.Now,
                Status = status,
                Amount = amount,
                UserID = userID,
                Target = target
            };
            _transactionService.AddNewTransaction(transaction);
        }
    }
}
