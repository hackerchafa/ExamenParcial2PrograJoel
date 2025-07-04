// Limpieza de duplicados y definición única de clase y usings
using ExamenApi.Models;
using ExamenApi.Models.Dtos;
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

        // GET: api/Budgets/all - obtener todos los budgets con sus categorías y gastos
        [HttpGet("all")]
        public async Task<IActionResult> GetAllBudgets()
        {
            var budgets = await _context.MonthlyBudgets
                .Include(b => b.Categories)
                    .ThenInclude(c => c.Expenses)
                .ToListAsync();
            return Ok(budgets);
        }

        // GET: api/Budgets?page=1&pageSize=10 - paginación
        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (!Request.Query.ContainsKey("page") || string.IsNullOrWhiteSpace(Request.Query["page"]))
                return BadRequest(new { error = "El parámetro 'page' es obligatorio y no puede estar vacío." });
            if (!Request.Query.ContainsKey("pageSize") || string.IsNullOrWhiteSpace(Request.Query["pageSize"]))
                return BadRequest(new { error = "El parámetro 'pageSize' es obligatorio y no puede estar vacío." });
            if (page <= 0)
                return BadRequest(new { error = "El parámetro 'page' debe ser un número entero mayor que 0." });
            if (pageSize <= 0)
                return BadRequest(new { error = "El parámetro 'pageSize' debe ser un número entero mayor que 0." });
            var budgets = await _context.MonthlyBudgets
                .Include(b => b.Categories)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            var total = await _context.MonthlyBudgets.CountAsync();
            return Ok(new { total, budgets });
        }

        [HttpPost]
        public async Task<IActionResult> Create(MonthlyBudgetCreateDto dto)
        {
            var budget = new MonthlyBudget
            {
                Month = dto.Month,
                Categories = dto.Categories?.Select(cat => new BudgetCategory
                {
                    Name = cat.Name,
                    Limit = cat.Limit,
                    MonthlyBudgetId = 0, // Se asignará por EF
                    MonthlyBudget = null!,
                    Expenses = new List<Expense>()
                }).ToList() ?? new List<BudgetCategory>()
            };
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MonthlyBudgetUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var budget = new MonthlyBudget
            {
                Id = dto.Id,
                Month = dto.Month,
                Categories = dto.Categories?.Select(cat => new BudgetCategory
                {
                    Id = cat.Id,
                    Name = cat.Name,
                    Limit = cat.Limit,
                    MonthlyBudgetId = cat.MonthlyBudgetId,
                    MonthlyBudget = null!,
                    Expenses = new List<Expense>()
                }).ToList() ?? new List<BudgetCategory>()
            };
            _context.Entry(budget).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.MonthlyBudgets.AnyAsync(b => b.Id == id))
                    return NotFound();
                else
                    throw;
            }
            return Ok(budget);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var budget = await _context.MonthlyBudgets.FindAsync(id);
            if (budget == null) return NotFound();
            _context.MonthlyBudgets.Remove(budget);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
