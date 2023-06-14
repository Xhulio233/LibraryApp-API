using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sample_crud_be_2.Models;

namespace sample_crud_be_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public CategoryController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoryList()
        {
            if (_applicationDbContext.Categories == null)
            {
                return NotFound();
            }
            return await _applicationDbContext.Categories.ToListAsync();
        }

        [HttpGet("getById")]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            if (_applicationDbContext.Categories == null)
            {
                return NotFound();
            }
            var category = await _applicationDbContext.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return category;
        }

        [HttpPost("create")]
        public async Task<ActionResult<Category>> Create(Category category)
        {
            _applicationDbContext.Categories.Add(category);
            await _applicationDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        [HttpPut("update")]
        public async Task<ActionResult> Update(int id, Category category)
        {

            _applicationDbContext.Entry(category).State = EntityState.Modified;
            try
            {
                await _applicationDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok();
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> Delete(int id)
        {
            if (_applicationDbContext.Categories == null)
            {
                return NotFound();
            }

            var category = await _applicationDbContext.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _applicationDbContext.Categories.Remove(category);
            await _applicationDbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
