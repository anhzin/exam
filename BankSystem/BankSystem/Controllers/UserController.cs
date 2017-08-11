using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BankSystem.Data;
using BankSystem.Models;
using BankSystem.Services;

namespace BankSystem.Controllers
{
    public class UserController : Controller
    {
        private ITransactionRepository transactionRepository;
        private IUserRepository userRepository;
        private readonly BankSystemContext _context;
        private Guid AccountNumber;
        public UserController(BankSystemContext context)
        {
            _context = context;
            this.userRepository = new UserRepository(_context);
            this.transactionRepository = new TransactionRepository(_context);
          
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .SingleOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> DetailProfile(Guid accountNumber)
        {
            if (AccountNumber == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .SingleOrDefaultAsync(m => m.AccountNumber == accountNumber);
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
                    _context.Add(user);
                    await _context.SaveChangesAsync();

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
                    var user1 = await _context.Users
                 .SingleOrDefaultAsync(m => m.AccountName.Equals(user.AccountName) && m.Password.Equals(user.Password));
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
       


        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.ID == id);
        }
    }
}
