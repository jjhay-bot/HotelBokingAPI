# 2. Creating Models

Models represent the data entities in your database. In Entity Framework Core, these are C# classes that map to database tables.

## Creating a Book Model

Create a `Models` folder if it doesn't exist, then add `Book.cs`:

```csharp
namespace MyApi.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
```

## Key Points

- **Id**: Primary key, auto-incremented by EF Core.
- **string.Empty**: Default value to avoid null reference exceptions.
- **DateTime**: Use UTC for consistency.
- **decimal**: For monetary values (better than float/double).

## Conventions

- Class name becomes table name (Book → Books).
- Properties become columns.
- EF Core infers types automatically.

## Next Step

[Setting Up Database Context](03-Database-Context.md)
