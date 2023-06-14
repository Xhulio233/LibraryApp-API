
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sample_crud_be_2.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sample_crud_be_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public AuthorController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthorList()
        {
            var authors = await _applicationDbContext.Authors.ToListAsync();

            if (authors == null)
            {
                return NotFound();
            }

            return authors;
        }

        [HttpGet("getById")]
        public async Task<ActionResult<Author>> GetAuthorById(int id)
        {
            var author = await _applicationDbContext.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            return author;
        }

        [HttpPost("create")]
        public async Task<ActionResult<Author>> Create(Author author)
        {
            try
            {
                _applicationDbContext.Authors.Add(author);
                await _applicationDbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAuthorById), new { id = author.Id }, author);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult> Update(int id, Author author)
        {
            var existingAuthor = await _applicationDbContext.Authors.FindAsync(id);

            if (existingAuthor == null)
            {
                return NotFound();
            }

            existingAuthor.Name = author.Name;
            existingAuthor.Bio = author.Bio;
            existingAuthor.CreatedAt = author.CreatedAt;
            existingAuthor.CreatedBy = author.CreatedBy;

            await _applicationDbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> Delete(int id)
        {
            var author = await _applicationDbContext.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            _applicationDbContext.Authors.Remove(author);
            await _applicationDbContext.SaveChangesAsync();

            return Ok();
        }
    }
}

