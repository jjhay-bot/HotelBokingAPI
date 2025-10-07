# 5. Adding DTOs

DTOs (Data Transfer Objects) separate your API contract from internal models, improving security and flexibility.

## Creating Book DTOs

Create `DTOs/BookDto.cs` and `DTOs/CreateBookDto.cs`:

```csharp
// DTOs/BookDto.cs
namespace MyApi.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
        public decimal Price { get; set; }
    }
}

// DTOs/CreateBookDto.cs
namespace MyApi.DTOs
{
    public class CreateBookDto
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
        public decimal Price { get; set; }
    }
}
```

## Setting Up AutoMapper

Install AutoMapper if not already done, then create `Mapping/BookProfile.cs`:

```csharp
using AutoMapper;
using MyApi.DTOs;
using MyApi.Models;

namespace MyApi.Mapping
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDto>();
            CreateMap<CreateBookDto, Book>();
        }
    }
}
```

Register in `Program.cs`:

```csharp
// ...existing code...
builder.Services.AddAutoMapper(typeof(Program));
// ...existing code...
```

## Updating Controller

Modify `BooksController.cs` to use DTOs:

```csharp
// ...existing code...
using MyApi.DTOs;
// ...existing code...

[HttpGet]
public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
{
    var books = await _context.Books.ToListAsync();
    return _mapper.Map<List<BookDto>>(books);
}

[HttpPost]
public async Task<ActionResult<BookDto>> CreateBook(CreateBookDto createBookDto)
{
    var book = _mapper.Map<Book>(createBookDto);
    _context.Books.Add(book);
    await _context.SaveChangesAsync();
    var bookDto = _mapper.Map<BookDto>(book);
    return CreatedAtAction(nameof(GetBook), new { id = book.Id }, bookDto);
}

// ...similarly for other methods...
```

## Key Points

- **Separation of Concerns**: Models for DB, DTOs for API.
- **AutoMapper**: Simplifies mapping between objects.
- **Security**: Exclude sensitive fields from DTOs.

## Next Step

[Entity Framework Migrations](06-Migrations.md)
