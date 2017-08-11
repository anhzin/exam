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
    public class TransactionsController : Controller
    {
        private ITransactionRepository transactionRepository;
        private IUserRepository userRepository;
        private readonly BankSystemContext _context;
        private static Guid AccountNumber;
        public TransactionsController(BankSystemContext context)
        {
            _context = context;
            this.transactionRepository = new TransactionRepository(_context);
            this.userRepository = new UserRepository(_context);
        }

        // GET: Transactions
        public async Task<IActionResult> Index(Guid accountNumber)
        {
            AccountNumber = accountNumber;
            var listItems = await _context.Transactions.Where(m => m.AccountNumber == accountNumber).ToListAsync();
            return View(listItems.OrderByDescending(t => t.CreatedDate));
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .SingleOrDefaultAsync(m => m.ID == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }
        

        private bool TransactionExists(Guid id)
        {
            return _context.Transactions.Any(e => e.ID == id);
        }

        // GET: Transactions/Deposite
        public IActionResult Deposite(Guid accountNumber)
            
        {
            AccountNumber = accountNumber;
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
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _context.Users.SingleOrDefaultAsync(m => m.AccountNumber == AccountNumber);
                    if (user == null)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to deposite. Try again, and if the problem persists contact your system administrator.");
                    }
                    else
                    {
                        try
                        {
                            user.Balance += amount;
                            _context.Users.Update(user);
                            try
                            {
                                transaction.ID = Guid.NewGuid();
                                transaction.Type = TransactionTypes.Deposite;
                                transaction.CreatedDate = DateTime.Now;
                                transaction.Status = true;
                                transaction.Amount = amount;
                                transaction.AccountNumber = AccountNumber;
                                _context.Transactions.Add(transaction);
                                await _context.SaveChangesAsync();
                            }
                            catch (DbUpdateConcurrencyException ex)
                            {
                                var entry = ex.Entries.Single();
                                var clientValues = (Transaction)entry.Entity;
                            }
                        }
                        catch (DbUpdateConcurrencyException ex)
                        {
                            transaction.ID = Guid.NewGuid();
                            transaction.Type = TransactionTypes.Deposite;
                            transaction.CreatedDate = DateTime.Now;
                            transaction.Status = false;
                            transaction.Amount = amount;
                            transaction.AccountNumber = AccountNumber;
                            _context.Transactions.Add(transaction);
                            await _context.SaveChangesAsync();
                            ModelState.AddModelError(string.Empty, "Unable to deposite. Try again, and if the problem persists contact your system administrator.");
                            return View(transaction);
                        }
                    }



                     return RedirectToAction("Index", new { accountNumber = user.AccountNumber });
                }
            }
            catch (Exception ex)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to deposite. Try again, and if the problem persists contact your system administrator.");
                return View(transaction);
            }

            return View(transaction);
        }

        // GET: Transactions/WithDraw
        public IActionResult WithDraw(Guid accountNumber)
        {
            AccountNumber = accountNumber;
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
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _context.Users.SingleOrDefaultAsync(m => m.AccountNumber == AccountNumber);
                    if (user == null)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to WithDraw. Try again, and if the problem persists contact your system administrator.");
                    }
                    else
                    {
                        try
                        {
                            if (user.Balance >= amount)
                            {
                                user.Balance -= amount;
                                _context.Users.Update(user);


                                try
                                {
                                    transaction.ID = Guid.NewGuid();
                                    transaction.Type = TransactionTypes.WithDraw;
                                    transaction.CreatedDate = DateTime.Now;
                                    transaction.Status = true;
                                    transaction.Amount = amount;
                                    transaction.AccountNumber = AccountNumber;
                                    _context.Transactions.Add(transaction);
                                    await _context.SaveChangesAsync();
                                }
                                catch (DbUpdateConcurrencyException ex)
                                {
                                    var entry = ex.Entries.Single();
                                    var clientValues = (Transaction)entry.Entity;
                                }
                            }
                            else
                            {
                                transaction.ID = Guid.NewGuid();
                                transaction.Type = TransactionTypes.WithDraw;
                                transaction.CreatedDate = DateTime.Now;
                                transaction.Status = false;
                                transaction.Amount = amount;
                                transaction.AccountNumber = AccountNumber;
                                _context.Transactions.Add(transaction);
                                await _context.SaveChangesAsync();
                                ModelState.AddModelError(string.Empty, "Unable to WithDraw. Your balance is not enought to withdraw");
                                return View(transaction);
                            }
                        }
                        catch (Exception ex)
                        {

                            ModelState.AddModelError(string.Empty, "Unable to WithDraw. Try again, and if the problem persists contact your system administrator.");
                            return View(transaction);
                        }
                    }



                    return RedirectToAction("Index", new { accountNumber = user.AccountNumber });
                }
            }
            catch (Exception ex)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to deposite. Try again, and if the problem persists contact your system administrator.");
                return View(transaction);
            }

            return View(transaction);
        }

        // GET: Transactions/Transfer
        public IActionResult Transfer(Guid accountNumber)
        {
            AccountNumber = accountNumber;
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
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _context.Users.SingleOrDefaultAsync(m => m.AccountNumber == AccountNumber);
                    var userTarget = await _context.Users.SingleOrDefaultAsync(m => m.AccountNumber == target);
                    if (user == null)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to Transfer. Try again, and if the problem persists contact your system administrator.");
                        return View(transaction);
                    }
                    else if(userTarget == null)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to Transfer. Cannot find the target account!");
                        return View(transaction);
                    }
                    else
                    {
                        try
                        {
                            if (user.Balance >= amount)
                            {
                                user.Balance -= amount;
                                _context.Users.Update(user);
                                userTarget.Balance += amount;
                                _context.Users.Update(userTarget);
                                try
                                {
                                    transaction.ID = Guid.NewGuid();
                                    transaction.Type = TransactionTypes.Transfer;
                                    transaction.CreatedDate = DateTime.Now;
                                    transaction.Status = true;
                                    transaction.Amount = amount;
                                    transaction.AccountNumber = AccountNumber;
                                    transaction.Target = userTarget.AccountNumber.ToString();
                                    _context.Transactions.Add(transaction);
                                    await _context.SaveChangesAsync();
                                }
                                catch (DbUpdateConcurrencyException ex)
                                {
                                    var entry = ex.Entries.Single();
                                    var clientValues = (Transaction)entry.Entity;
                                }
                            }
                            else
                            {
                                transaction.ID = Guid.NewGuid();
                                transaction.Type = TransactionTypes.Transfer;
                                transaction.CreatedDate = DateTime.Now;
                                transaction.Status = false;
                                transaction.Amount = amount;
                                transaction.AccountNumber = AccountNumber;
                                transaction.Target = userTarget.AccountNumber.ToString();
                                _context.Transactions.Add(transaction);
                                await _context.SaveChangesAsync();
                                ModelState.AddModelError(string.Empty, "Unable to Transfer. Your balance is not enought to Transfer");
                                return View(transaction);
                            }
                        }
                        catch (Exception ex)
                        {

                            ModelState.AddModelError(string.Empty, "Unable to Transfer. Try again, and if the problem persists contact your system administrator.");
                            return View(transaction);
                        }
                    }

                    return RedirectToAction("Index", new { accountNumber = user.AccountNumber });
                }
            }
            catch (Exception ex)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to deposite. Try again, and if the problem persists contact your system administrator.");
                return View(transaction);
            }

            return View(transaction);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> DetailProfile()
        {
            return RedirectToAction("DetailProfile", "User", new { accountNumber = AccountNumber });
        }

    }
}
