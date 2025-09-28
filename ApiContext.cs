using Microsoft.EntityFrameworkCore;

public class ApiContext : DbContext
{
    public ApiContext(DbContextOptions<ApiContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<RoomType> RoomTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Age);

        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, FirstName = "Alice", LastName = "Smith", Email = "alice@example.com", Age = 25, Role = "Admin", IsActive = true, PasswordHash = "password", CreatedAt = new DateTime(2025, 9, 28, 0, 0, 0, DateTimeKind.Utc) },
            new User { Id = 2, FirstName = "Bob", LastName = "Jones", Email = "bob@example.com", Age = 30, Role = "User", IsActive = true, PasswordHash = "password", CreatedAt = new DateTime(2025, 9, 28, 0, 0, 0, DateTimeKind.Utc) },
            new User { Id = 3, FirstName = "Charlie", LastName = "Brown", Email = "charlie@example.com", Age = 28, Role = "User", IsActive = true, PasswordHash = "password", CreatedAt = new DateTime(2025, 9, 28, 0, 0, 0, DateTimeKind.Utc) },
            new User { Id = 4, FirstName = "Diana", LastName = "Prince", Email = "diana@example.com", Age = 32, Role = "Manager", IsActive = true, PasswordHash = "password", CreatedAt = new DateTime(2025, 9, 28, 0, 0, 0, DateTimeKind.Utc) },
            new User { Id = 5, FirstName = "Eve", LastName = "Adams", Email = "eve@example.com", Age = 27, Role = "User", IsActive = true, PasswordHash = "password", CreatedAt = new DateTime(2025, 9, 28, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
