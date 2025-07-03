using ExamenApi.Models;
using ExamenApi.Repositories;

namespace ExamenApi.Services
{
    public interface IBudgetCategoryService
    {
        Task<IEnumerable<BudgetCategory>> GetAllAsync(int page, int pageSize);
        Task<BudgetCategory> GetByIdAsync(int id);
        Task<BudgetCategory> AddAsync(BudgetCategory category);
        Task<BudgetCategory> UpdateAsync(BudgetCategory category);
        Task<bool> DeleteAsync(int id);
        Task<int> CountAsync();
    }

    public class BudgetCategoryService : IBudgetCategoryService
    {
        private readonly IBudgetCategoryRepository _repo;
        public BudgetCategoryService(IBudgetCategoryRepository repo)
        {
            _repo = repo;
        }
        public Task<IEnumerable<BudgetCategory>> GetAllAsync(int page, int pageSize) => _repo.GetAllAsync(page, pageSize);
        public Task<BudgetCategory> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public async Task<BudgetCategory> AddAsync(BudgetCategory category)
        {
            if (await _repo.ExistsByNameAsync(category.Name, category.MonthlyBudgetId))
                throw new Exception("Category name already exists in this budget.");
            return await _repo.AddAsync(category);
        }
        public Task<BudgetCategory> UpdateAsync(BudgetCategory category) => _repo.UpdateAsync(category);
        public async Task<bool> DeleteAsync(int id)
        {
            if (await _repo.HasExpensesAsync(id))
                throw new Exception("Cannot delete category with expenses.");
            return await _repo.DeleteAsync(id);
        }
        public Task<int> CountAsync() => _repo.CountAsync();
    }
}
