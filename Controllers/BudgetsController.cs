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
    public class BudgetsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public BudgetsController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Create(MonthlyBudget budget)
        {
            if (budget.Categories != null)
            {
                foreach (var cat in budget.Categories)
                {
                    if (await _context.BudgetCategories.AnyAsync(c => c.Name == cat.Name && c.MonthlyBudgetId == budget.Id))
                        return BadRequest($"Category name '{cat.Name}' already exists in this budget.");
                }
            }
            _context.MonthlyBudgets.Add(budget);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = budget.Id }, budget);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var budget = await _context.MonthlyBudgets.Include(b => b.Categories).FirstOrDefaultAsync(b => b.Id == id);
            if (budget == null) return NotFound();
            return Ok(budget);
        }
    }
}
