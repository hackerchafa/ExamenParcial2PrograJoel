using ExamenApi.Models;
using ExamenApi.Models.Dtos;
using ExamenApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamenApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BudgetCategoryController : ControllerBase
    {
        private readonly IBudgetCategoryService _service;
        public BudgetCategoryController(IBudgetCategoryService service)
        {
            _service = service;
        }
        // GET: api/BudgetCategory/all - obtener todas las categorías sin paginación
        [HttpGet("all")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _service.GetAllAsync(1, int.MaxValue);
            return Ok(categories);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10)
        {
            // Validar que page y pageSize no sean nulos, vacíos o menores a 1
            if (!Request.Query.ContainsKey("page") || string.IsNullOrWhiteSpace(Request.Query["page"]))
                return BadRequest(new { error = "El parámetro 'page' es obligatorio y no puede estar vacío." });
            if (!Request.Query.ContainsKey("pageSize") || string.IsNullOrWhiteSpace(Request.Query["pageSize"]))
                return BadRequest(new { error = "El parámetro 'pageSize' es obligatorio y no puede estar vacío." });

            if (page <= 0)
                return BadRequest(new { error = "El parámetro 'page' debe ser un número entero mayor que 0." });
            if (pageSize <= 0)
                return BadRequest(new { error = "El parámetro 'pageSize' debe ser un número entero mayor que 0." });

            var items = await _service.GetAllAsync(page, pageSize);
            var total = await _service.CountAsync();
            return Ok(new { total, items });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            // Validar nulo, vacío o solo espacios
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(new { error = "El id no puede estar vacío, contener solo espacios ni ser nulo. Solo se aceptan números enteros mayores a 0." });

            // Validar que solo contenga dígitos
            if (!id.All(char.IsDigit))
                return BadRequest(new { error = "El id solo puede contener números enteros mayores a 0. No se aceptan letras, espacios ni símbolos." });

            if (!int.TryParse(id, out int parsedId) || parsedId <= 0)
                return BadRequest(new { error = "El id debe ser un número entero mayor que 0." });

            var item = await _service.GetByIdAsync(parsedId);
            if (item == null) return NotFound();
            return Ok(item);
        }
        [HttpPost]
        public async Task<IActionResult> Create(BudgetCategoryCreateDto dto)
        {
            try
            {
                var category = new BudgetCategory
                {
                    Name = dto.Name,
                    Limit = dto.Limit,
                    MonthlyBudgetId = dto.MonthlyBudgetId,
                    MonthlyBudget = null!, // Será asignado por EF al guardar
                    Expenses = new List<Expense>()
                };
                var created = await _service.AddAsync(category);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BudgetCategoryUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var category = new BudgetCategory
            {
                Id = dto.Id,
                Name = dto.Name,
                Limit = dto.Limit,
                MonthlyBudgetId = dto.MonthlyBudgetId,
                MonthlyBudget = null!, // Será asignado por EF al guardar
                Expenses = new List<Expense>()
            };
            var updated = await _service.UpdateAsync(category);
            return Ok(updated);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            // Validar nulo, vacío o solo espacios
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(new { error = "El id no puede estar vacío, contener solo espacios ni ser nulo. Solo se aceptan números enteros mayores a 0." });

            // Validar que solo contenga dígitos
            if (!id.All(char.IsDigit))
                return BadRequest(new { error = "El id solo puede contener números enteros mayores a 0. No se aceptan letras, espacios ni símbolos." });

            if (!int.TryParse(id, out int parsedId) || parsedId <= 0)
                return BadRequest(new { error = "El id debe ser un número entero mayor que 0." });

            try
            {
                var result = await _service.DeleteAsync(parsedId);
                if (!result) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
