using ExamenApi.Models;
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
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10)
        {
            var items = await _service.GetAllAsync(page, pageSize);
            var total = await _service.CountAsync();
            return Ok(new { total, items });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }
        [HttpPost]
        public async Task<IActionResult> Create(BudgetCategory category)
        {
            try
            {
                var created = await _service.AddAsync(category);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BudgetCategory category)
        {
            if (id != category.Id) return BadRequest();
            var updated = await _service.UpdateAsync(category);
            return Ok(updated);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);
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
