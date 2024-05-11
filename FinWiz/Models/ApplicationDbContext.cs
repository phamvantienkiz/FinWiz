using Microsoft.EntityFrameworkCore;
using FinWiz.Models;

namespace FinWiz.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options) : base(options)
        { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<ElectricityBill> ElectricityBill { get; set;}
        public DbSet<WaterBill> WaterBill { get; set; }
        public DbSet<FinWiz.Models.TransactionViewModel> TransactionViewModel { get; set; } = default!;
    }
}
