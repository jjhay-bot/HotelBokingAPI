# 6. Entity Framework Migrations

Migrations allow you to version your database schema changes.

## Creating Initial Migration

1. **Add Migration**:

   ```bash
   dotnet ef migrations add InitialCreate
   ```

2. **Update Database**:

   ```bash
   dotnet ef database update
   ```

This creates the `Books` table in your PostgreSQL database.

## Adding Data Seeding (Optional)

In `MyApiContext.cs`, add seeding in `OnModelCreating`:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // ...existing code...

    modelBuilder.Entity<Book>().HasData(
        new Book { Id = 1, Title = "Sample Book", Author = "Author", ISBN = "1234567890123", PublishedDate = DateTime.UtcNow, Price = 19.99m }
    );
}
```

Then create and run a new migration:

```bash
dotnet ef migrations add AddBookSeed
dotnet ef database update
```

## Key Commands

- `dotnet ef migrations add <Name>`: Create migration
- `dotnet ef database update`: Apply migrations
- `dotnet ef migrations remove`: Remove last migration
- `dotnet ef migrations list`: List migrations

## Next Step

[Running and Testing the API](07-Running-API.md)
