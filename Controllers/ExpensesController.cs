// ...existing code...
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
    public class ExpensesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ExpensesController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Create(ExpenseCreateDto dto)
        {
            var category = await _context.BudgetCategories.Include(c => c.Expenses).FirstOrDefaultAsync(c => c.Id == dto.BudgetCategoryId);
            if (category == null) return BadRequest("Category not found");
            var total = category.Expenses.Sum(e => e.Amount) + dto.Amount;
            if (total > category.Limit)
                return BadRequest("Expense exceeds the category limit");
            var expense = new Expense
            {
                Amount = dto.Amount,
                BudgetCategoryId = dto.BudgetCategoryId,
                Date = dto.Date,
                BudgetCategory = null! // Será asignado por EF
            };
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            return CreatedAtAction(null, new { id = expense.Id }, expense);
        }

        // GET: api/Expenses/all - obtener todos los gastos
        [HttpGet("all")]
        public async Task<IActionResult> GetAllExpenses()
        {
            var expenses = await _context.Expenses.Include(e => e.BudgetCategory).ToListAsync();
            return Ok(expenses);
        }

        // GET: api/Expenses?page=1&pageSize=10 - paginación
        [HttpGet]
        public async Task<IActionResult> GetPagedExpenses([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (!Request.Query.ContainsKey("page") || string.IsNullOrWhiteSpace(Request.Query["page"]))
                return BadRequest(new { error = "El parámetro 'page' es obligatorio y no puede estar vacío." });
            if (!Request.Query.ContainsKey("pageSize") || string.IsNullOrWhiteSpace(Request.Query["pageSize"]))
                return BadRequest(new { error = "El parámetro 'pageSize' es obligatorio y no puede estar vacío." });
            if (page <= 0)
                return BadRequest(new { error = "El parámetro 'page' debe ser un número entero mayor que 0." });
            if (pageSize <= 0)
                return BadRequest(new { error = "El parámetro 'pageSize' debe ser un número entero mayor que 0." });
            var expenses = await _context.Expenses
                .Include(e => e.BudgetCategory)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            var total = await _context.Expenses.CountAsync();
            return Ok(new { total, expenses });
        }

        // GET: api/Expenses/{id} - buscar por id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var expense = await _context.Expenses.Include(e => e.BudgetCategory).FirstOrDefaultAsync(e => e.Id == id);
            if (expense == null) return NotFound();
            return Ok(expense);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ExpenseUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var expense = new Expense
            {
                Id = dto.Id,
                Amount = dto.Amount,
                BudgetCategoryId = dto.BudgetCategoryId,
                Date = dto.Date,
                BudgetCategory = null!
            };
            _context.Entry(expense).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Expenses.AnyAsync(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }
            return Ok(expense);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null) return NotFound();
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
