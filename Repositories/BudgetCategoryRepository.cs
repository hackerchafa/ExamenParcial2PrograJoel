using ExamenApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamenApi.Repositories
{
    public interface IBudgetCategoryRepository
    {
        Task<IEnumerable<BudgetCategory>> GetAllAsync(int page, int pageSize);
        Task<BudgetCategory> GetByIdAsync(int id);
        Task<BudgetCategory> AddAsync(BudgetCategory category);
        Task<BudgetCategory> UpdateAsync(BudgetCategory category);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsByNameAsync(string name, int monthlyBudgetId);
        Task<bool> HasExpensesAsync(int categoryId);
        Task<int> CountAsync();
    }

    public class BudgetCategoryRepository : IBudgetCategoryRepository
    {
        private readonly Data.AppDbContext _context;
        public BudgetCategoryRepository(Data.AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<BudgetCategory>> GetAllAsync(int page, int pageSize)
        {
            return await _context.BudgetCategories
                .Include(c => c.Expenses)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<BudgetCategory> GetByIdAsync(int id)
        {
            return await _context.BudgetCategories.Include(c => c.Expenses).FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<BudgetCategory> AddAsync(BudgetCategory category)
        {
            _context.BudgetCategories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }
        public async Task<BudgetCategory> UpdateAsync(BudgetCategory category)
        {
            _context.BudgetCategories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.BudgetCategories.Include(c => c.Expenses).FirstOrDefaultAsync(c => c.Id == id);
            if (category == null || category.Expenses.Any()) return false;
            _context.BudgetCategories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ExistsByNameAsync(string name, int monthlyBudgetId)
        {
            return await _context.BudgetCategories.AnyAsync(c => c.Name == name && c.MonthlyBudgetId == monthlyBudgetId);
        }
        public async Task<bool> HasExpensesAsync(int categoryId)
        {
            return await _context.Expenses.AnyAsync(e => e.BudgetCategoryId == categoryId);
        }
        public async Task<int> CountAsync()
        {
            return await _context.BudgetCategories.CountAsync();
        }
    }
}
