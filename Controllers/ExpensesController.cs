using ExamenApi.Models;
using ExamenApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamenApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExpensesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ExpensesController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Create(Expense expense)
        {
            var category = await _context.BudgetCategories.Include(c => c.Expenses).FirstOrDefaultAsync(c => c.Id == expense.BudgetCategoryId);
            if (category == null) return BadRequest("Category not found");
            var total = category.Expenses.Sum(e => e.Amount) + expense.Amount;
            if (total > category.Limit)
                return BadRequest("Expense exceeds the category limit");
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            return CreatedAtAction(null, new { id = expense.Id }, expense);
        }
    }
}
