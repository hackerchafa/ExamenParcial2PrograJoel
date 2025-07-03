using Microsoft.EntityFrameworkCore;
using ExamenApi.Models;

namespace ExamenApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<MonthlyBudget> MonthlyBudgets { get; set; }
        public DbSet<BudgetCategory> BudgetCategories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
    }
}
