using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using sample_crud_be_2.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace sample_crud_be_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IOptions<FileUploadOptions> _fileUploadOptions;

        public BookController(ApplicationDbContext applicationDbContext, IOptions<FileUploadOptions> fileUploadOptions)
        {
            _applicationDbContext = applicationDbContext;
            _fileUploadOptions = fileUploadOptions;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBook()
        {
            var books = await _applicationDbContext.Books.ToListAsync();

            if (books == null)
            {
              
                return new List<Book>();
            }

           
            books = books.Where(b => b != null).ToList();

            return books;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] BookCreateDto dto)
        {
            var author = await _applicationDbContext.Authors.FindAsync(dto.AuthorId);
            if (author == null)
            {
                return BadRequest("Author not found");
            }

            var categories = await _applicationDbContext.Categories
                .Where(category => dto.CategoryIds.Contains(category.Id))
                .ToListAsync();

            if (categories.Count != dto.CategoryIds.Count || categories.Count == 0)
            {
                return BadRequest("One or more categories not found");
            }

            var book = new Book
            {
                Name = dto.Name,
                Description = dto.Description,
                CreatedAt = DateTime.Now.ToString(),
                CreatedBy = string.Empty,
                AuthorId = author.Id,
                Author = author,
                Categories = categories
            };

            _applicationDbContext.Books.Add(book);
            await _applicationDbContext.SaveChangesAsync();

            if (dto.Image != null && dto.Image.Length > 0)
            {
                var fileName = Path.GetFileName(dto.Image.FileName);
                var filePath = Path.Combine("BOOKS", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(stream);
                }

                book.Image = filePath;
                await _applicationDbContext.SaveChangesAsync();
            }
            var response = new
            {
                book.Id,
                book.Name,
                book.Description,
                Image = Url.Content("~/BOOKS/" + Path.GetFileName(book.Image)),
                Categories = book.Categories.Select(category => category.Name)
            };

            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, response);
        }




        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBookById(int id)
        {
            var book = await _applicationDbContext.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        

        [HttpPut("update/{id}")]
        public async Task<ActionResult> Update(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            var existingBook = await _applicationDbContext.Books.FindAsync(id);
            if (existingBook == null)
            {
                return NotFound();
            }

            existingBook.Name = book.Name;
            existingBook.Description = book.Description;
            existingBook.Image = book.Image;
            existingBook.AuthorId = book.AuthorId;
            existingBook.Categories = book.Categories;

            try
            {
                await _applicationDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var book = await _applicationDbContext.Books
                .Include(b => b.Categories)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            _applicationDbContext.Books.Remove(book);
            await _applicationDbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("getBooksByAuthorId")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksByAuthorId(int authorId)
        {
            var books = _applicationDbContext.Books
                .Where(book => book.Author != null && book.Author.Id == authorId)
                .ToList();

            return books;
        }
    }
}
