using FinWiz.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FinWiz.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //Last 7 days
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Today;

            List<Expense> selectExpense = await _context.Expenses
                .Include(x => x.Category)
                .Where(d => d.Date >= StartDate && d.Date <= EndDate).ToListAsync();

            List<Income> selectIncome = await _context.Incomes
                .Include(x => x.Category)
                .Where(d => d.Date >= StartDate && d.Date <= EndDate).ToListAsync();

            //Total Income
            decimal totalIncome = selectIncome.Sum(a => a.Amount);
            CultureInfo culture = CultureInfo.CreateSpecificCulture("vi-VN"); // Thay đổi từ "en-US" sang "vi-VN"
            culture.NumberFormat.CurrencyNegativePattern = 1;
            ViewBag.TotalIncome = String.Format(culture, "{0:#,##0.0}", totalIncome);

            //Total Expense
            decimal totalExpense = selectExpense.Sum(a => a.Amount);
            
            ViewBag.TotalExpense = String.Format(culture, "{0:#,##0.0}", totalExpense);

            

            //Balance
            var Balance = totalIncome - totalExpense;
            ViewBag.Balance = String.Format(culture, "{0:#,##0.0}", Balance);

            //Doughnut Chart - Expense By Category
            ViewBag.DoughnutChartData = selectExpense
                .GroupBy(c => c.CategoryId)
                .Select(k => new
                {
                    categoryTitleWithIcon = k.First().Category.Icon + " " + k.First().Category.Title,
                    amount = k.Sum(j => j.Amount),
                    formatterAmount = k.Sum(j => j.Amount).ToString("C0"),
                }).OrderByDescending(o => o.amount).ToList();

            //Spline Chart - Income vs Expense

            //Income
            List<SplineChartData> IncomeSummary = selectIncome
                .GroupBy(c => c.Date)
                .Select(f => new SplineChartData()
                {
                    day = f.First().Date.ToString("dd-MMM"),
                    income = f.Sum(c => c.Amount)
                }).ToList();

            //Expense
            List<SplineChartData> ExpenseSummary = selectExpense
                .GroupBy(c => c.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    expense = k.Sum(c => c.Amount)
                }).ToList();

            //Combine Income & Expense
            string[] Last7Days = Enumerable.Range(0, 7)
                .Select(i => StartDate.AddDays(i).ToString("dd-MMM"))
                .ToArray();

            ViewBag.SplineChartData = from day in Last7Days
                                      join income in IncomeSummary on day equals income.day into dayIncomeJoined
                                      from income in dayIncomeJoined.DefaultIfEmpty()
                                      join expense in ExpenseSummary on day equals expense.day into expenseJoined
                                      from expense in expenseJoined.DefaultIfEmpty()
                                      select new
                                      {
                                          day = day,
                                          income = income == null ? 0 : income.income,
                                          expense = expense == null ? 0 : expense.expense,
                                      };



            return View();
        }

        public class SplineChartData
        {
            public string day;
            public decimal income;
            public decimal expense;

        }
    }
}
