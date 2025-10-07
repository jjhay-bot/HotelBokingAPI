# 3. Setting Up Database Context

The DbContext is the main class that coordinates Entity Framework functionality for a given data model.

## Creating the DbContext

Create a `Data` folder and add `MyApiContext.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using MyApi.Models;

namespace MyApi.Data
{
    public class MyApiContext : DbContext
    {
        public MyApiContext(DbContextOptions<MyApiContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure entities here if needed
            
            // Index for performance
            modelBuilder.Entity<Book>()
                .HasIndex(b => b.Author);

            // Property configurations
            modelBuilder.Entity<Book>()
                .Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Book>()
                .Property(b => b.Author)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Book>()
                .Property(b => b.ISBN)
                .IsRequired()
                .HasMaxLength(13);

            // Example: One-to-Many relationship (Book belongs to Publisher)
            // Assuming Publisher entity exists
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Publisher)
                .WithMany(p => p.Books)
                .HasForeignKey(b => b.PublisherId)
                .OnDelete(DeleteBehavior.Restrict);

            // Example: Many-to-Many relationship (Book to Categories)
            // Assuming Category entity exists
            modelBuilder.Entity<Book>()
                .HasMany(b => b.Categories)
                .WithMany(c => c.Books)
                .UsingEntity(j => j.ToTable("BookCategories"));

            // Example: Self-referencing relationship (Book series)
            modelBuilder.Entity<Book>()
                .HasOne(b => b.ParentBook)
                .WithMany(b => b.ChildBooks)
                .HasForeignKey(b => b.ParentBookId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
```

## Configuring in Program.cs

Update `Program.cs` to register the DbContext:

```csharp
// ...existing code...

builder.Services.AddDbContext<MyApiContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// ...existing code...
```

## Connection String

Add to `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=MyApiDb;Username=youruser;Password=yourpassword"
  }
}
```

## Key Points

- **DbSet<Book>**: Represents the Books table.
- **OnModelCreating**: Configure constraints, relationships, etc.
- **UseNpgsql**: For PostgreSQL (change for other databases).

## Next Step

[Creating Controllers](04-Creating-Controllers.md)
