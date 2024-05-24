using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinWiz.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace FinWiz.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly List<TransactionViewModel> listTransaction;

        public TransactionController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Transaction
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            PopulateCategories();
            var expenses = _context.Expenses
                .Include(c => c.Category)
                .Where(u => u.UserId == user.Id).ToList();

            var incomes = _context.Incomes
                .Include(c => c.Category)
                .Where(u => u.UserId == user.Id).ToList();

            var listTransaction = new List<TransactionViewModel>();

            foreach (var expense in expenses)
            {
                listTransaction.Add(new TransactionViewModel(expense));
            }

            foreach (var income in incomes)
            {
                listTransaction.Add(new TransactionViewModel(income));
            }

            return View(listTransaction);
        }

        // GET: Transaction/Details/5
        /*public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactionViewModel = await _context.TransactionViewModel
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.TransacionId == id);
            if (transactionViewModel == null)
            {
                return NotFound();
            }

            return View(transactionViewModel);
        }*/

        // GET: Transaction/Add
        public IActionResult Add(int id = 0/*, string type = "Expense"*/)
        {
            PopulateCategories();
            string type = HttpContext.Request.Query["Type"];
            if (id == 0)
            {
                return View(new TransactionViewModel());
            }
            else
            {
                if (type.Equals("Expense"))
                {
                    var expense = _context.Expenses.Find(id);
                    var transaction = new TransactionViewModel(expense);
                    return View(transaction);
                }
                else
                {
                    var income = _context.Incomes.Find(id);
                    var transaction = new TransactionViewModel(income);

                    return View(transaction);
                }

            }
        }

        // POST: Transaction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("Type,TransacionId,UserId,CategoryId,Amount,Note,Date")] TransactionViewModel transactionViewModel)
        {
            if (!ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (transactionViewModel.TransactionId == 0)
                {
                    if (transactionViewModel.Type.Equals("Expense"))
                    {
                        var newExpense = new Expense
                        {
                            UserId = user.Id,
                            CategoryId = transactionViewModel.CategoryId,
                            Category = transactionViewModel.Category,
                            Amount = transactionViewModel.Amount,
                            Note = transactionViewModel.Note,
                            Date = transactionViewModel.Date
                        };
                        _context.Expenses.Add(newExpense);
                    }
                    else if (transactionViewModel.Type.Equals("Income"))
                    {
                        var newIncome = new Income
                        {
                            UserId = user.Id,
                            CategoryId = transactionViewModel.CategoryId,
                            Category = transactionViewModel.Category,
                            Amount = transactionViewModel.Amount,
                            Note = transactionViewModel.Note,
                            Date = transactionViewModel.Date
                        };
                        _context.Incomes.Add(newIncome);    
                    }
                    /*await _context.SaveChangesAsync();
                    return RedirectToAction("Index");*/
                } 
                
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            PopulateCategories();
            return View(transactionViewModel);
            //return RedirectToAction("Index");
        }

        // GET: Transaction/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            PopulateCategories();
            string type = HttpContext.Request.Query["Type"];
            if (id == 0)
            {
                return View(new TransactionViewModel());
            }
            else
            {
                if (type.Equals("Expense"))
                {
                    var expense = _context.Expenses.Find(id);
                    var transaction = new TransactionViewModel(expense);
                    return View(transaction);
                }
                else
                {
                    var income = _context.Incomes.Find(id);
                    var transaction = new TransactionViewModel(income);

                    return View(transaction);
                }

            }
        }

        // POST: Transaction/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Type,TransacionId,UserId,CategoryId,Amount,Note,Date")] TransactionViewModel transactionViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (transactionViewModel.Type.Equals("Expense"))
                {
                    var expense = _context.Expenses.Find(id);
                    if (expense != null)
                    {
                        expense.CategoryId = transactionViewModel.CategoryId;
                        expense.Category = transactionViewModel.Category;
                        expense.Amount = transactionViewModel.Amount;
                        expense.Note = transactionViewModel.Note;
                        expense.Date = transactionViewModel.Date;

                        _context.Expenses.Update(expense);
                    } else
                    {
                        var newExpense = new Expense
                        {
                            UserId = user.Id,
                            CategoryId = transactionViewModel.CategoryId,
                            Category = transactionViewModel.Category,
                            Amount = transactionViewModel.Amount,
                            Note = transactionViewModel.Note,
                            Date = transactionViewModel.Date
                        };
                        _context.Expenses.Add(newExpense);

                        var isIncome = _context.Incomes.Find(id);
                        _context.Incomes.Remove(isIncome);
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else if (transactionViewModel.Type.Equals("Income"))
                {
                    var income = _context.Incomes.Find(id);
                    if (income != null)
                    {
                        income.CategoryId = transactionViewModel.CategoryId;
                        income.Category = transactionViewModel.Category;
                        income.Amount = transactionViewModel.Amount;
                        income.Note = transactionViewModel.Note;
                        income.Date = transactionViewModel.Date;

                        _context.Incomes.Update(income);
                    }
                    else
                    {
                        var newIncome = new Income
                        {
                            UserId = user.Id,
                            CategoryId = transactionViewModel.CategoryId,
                            Category = transactionViewModel.Category,
                            Amount = transactionViewModel.Amount,
                            Note = transactionViewModel.Note,
                            Date = transactionViewModel.Date
                        };
                        _context.Incomes.Add(newIncome);

                        var isExpense = _context.Expenses.Find(id);
                        _context.Expenses.Remove(isExpense);
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            PopulateCategories();
            return View(transactionViewModel);
        }

        // GET: Transaction/Delete/5
        /*public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactionViewModel = await _context.TransactionViewModel
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.TransacionId == id);
            if (transactionViewModel == null)
            {
                return NotFound();
            }

            return View(transactionViewModel);
        }*/

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string type = HttpContext.Request.Query["Type"];
            if (type.Equals("Expense"))
            {
                var isExpense = await _context.Expenses.FindAsync(id);
                if (isExpense != null)
                {
                    _context.Expenses.Remove(isExpense);
                }
                _context.SaveChanges();
                return RedirectToAction("Index");
            } 
            else if (type.Equals("Income"))
            {
                var isIncome = await _context.Incomes.FindAsync(id);
                if (isIncome != null)
                {
                    _context.Incomes.Remove(isIncome);
                }
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction(nameof(Index));

            /*var transactionViewModel = await _context.TransactionViewModel.FindAsync(id);
            if (transactionViewModel != null)
            {
                _context.TransactionViewModel.Remove(transactionViewModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));*/
        }

/*        private bool TransactionViewModelExists(int id)
        {
            return _context.TransactionViewModel.Any(e => e.TransactionId == id);
        }*/

        [NonAction]
        public void PopulateCategories()
        {
            var CategoryCollection = _context.Categories.ToList();
            Category DefaultCategory = new Category() { CategoryId = 0, Title = "Choose a Category" };
            CategoryCollection.Insert(0, DefaultCategory);
            ViewBag.Categories = CategoryCollection;
        }
    }
}
