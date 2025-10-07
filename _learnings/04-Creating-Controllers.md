# 4. Creating Controllers

Controllers handle HTTP requests and return responses. They contain the CRUD logic.

## Creating BooksController

Create `Controllers/BooksController.cs`:

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using MyApi.Data;
using MyApi.Models;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly MyApiContext _context;

        public BooksController(MyApiContext context)
        {
            _context = context;
        }

        // GET: api/books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await _context.Books.ToListAsync();
        }

        // GET: api/books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();
            return book;
        }

        // POST: api/books
        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        // PUT: api/books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, Book book)
        {
            if (id != book.Id) return BadRequest();
            _context.Entry(book).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id)) return NotFound();
                throw;
            }
            return NoContent();
        }

        // PATCH: api/books/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchBook(int id, JsonPatchDocument<Book> patchDoc)
        {
            if (patchDoc == null) return BadRequest();
            
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();
            
            patchDoc.ApplyTo(book, ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id)) return NotFound();
                throw;
            }
            return NoContent();
        }

        // DELETE: api/books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
```

## Key Points

- **[ApiController]**: Enables automatic model validation and 400 responses.
- **Dependency Injection**: DbContext injected via constructor.
- **Async/Await**: For non-blocking database operations.
- **HTTP Methods**: GET (read), POST (create), PUT (full update), PATCH (partial update), DELETE (remove).
- **JsonPatchDocument**: For PATCH operations to update only specific fields.
- **HTTP Status Codes**: 200 OK, 201 Created, 204 No Content, 404 Not Found, etc.

## Next Step

[Adding DTOs](05-Adding-DTOs.md)
