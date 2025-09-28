using Microsoft.EntityFrameworkCore;

public class ApiContext : DbContext
{
    public ApiContext(DbContextOptions<ApiContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<RoomType> RoomTypes { get; set; }

    public DbSet<Room> Rooms { get; set; }

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

        modelBuilder.Entity<RoomType>().HasData(
            new RoomType { Id = 1, Name = "Single", Description = "Single bed room" },
            new RoomType { Id = 2, Name = "Double", Description = "Double bed room" },
            new RoomType { Id = 3, Name = "Suite", Description = "Suite with extra amenities" },
            new RoomType { Id = 4, Name = "Deluxe", Description = "Deluxe room with premium features" },
            new RoomType { Id = 5, Name = "Family", Description = "Family room for multiple guests" },
            new RoomType { Id = 6, Name = "Penthouse", Description = "Top floor penthouse suite" },
            new RoomType { Id = 7, Name = "Studio", Description = "Studio room with kitchenette" },
            new RoomType { Id = 8, Name = "Executive", Description = "Executive room for business travelers" },
            new RoomType { Id = 9, Name = "Accessible", Description = "Accessible room for guests with disabilities" },
            new RoomType { Id = 10, Name = "Presidential Suite", Description = "Luxurious presidential suite" }
        );

        modelBuilder.Entity<Room>().HasData(
            new Room { Id = 1, RoomTypeId = 1, RoomNumber = 101, PricePerNight = 100, Capacity = 1, BedType = "Single", Size = "20 sq m", Floor = 1, Status = "available", Amenities = "WiFi,TV,Desk", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Room { Id = 2, RoomTypeId = 2, RoomNumber = 102, PricePerNight = 150, Capacity = 2, BedType = "Double", Size = "30 sq m", Floor = 1, Status = "available", Amenities = "WiFi,TV,Mini Bar", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Room { Id = 3, RoomTypeId = 3, RoomNumber = 201, PricePerNight = 200, Capacity = 2, BedType = "Queen", Size = "35 sq m", Floor = 2, Status = "occupied", Amenities = "WiFi,TV,Balcony", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Room { Id = 4, RoomTypeId = 4, RoomNumber = 202, PricePerNight = 250, Capacity = 2, BedType = "King", Size = "40 sq m", Floor = 2, Status = "maintenance", Amenities = "WiFi,TV,Mini Bar,Balcony", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Room { Id = 5, RoomTypeId = 5, RoomNumber = 301, PricePerNight = 180, Capacity = 4, BedType = "Double", Size = "45 sq m", Floor = 3, Status = "available", Amenities = "WiFi,TV,Kitchenette", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Room { Id = 6, RoomTypeId = 6, RoomNumber = 302, PricePerNight = 500, Capacity = 2, BedType = "King", Size = "80 sq m", Floor = 3, Status = "available", Amenities = "WiFi,TV,Mini Bar,City View", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Room { Id = 7, RoomTypeId = 7, RoomNumber = 401, PricePerNight = 120, Capacity = 1, BedType = "Single", Size = "25 sq m", Floor = 4, Status = "available", Amenities = "WiFi,TV,Desk", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Room { Id = 8, RoomTypeId = 8, RoomNumber = 402, PricePerNight = 300, Capacity = 2, BedType = "King", Size = "60 sq m", Floor = 4, Status = "available", Amenities = "WiFi,TV,Mini Bar,Balcony,City View", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Room { Id = 9, RoomTypeId = 9, RoomNumber = 501, PricePerNight = 110, Capacity = 2, BedType = "Twin", Size = "28 sq m", Floor = 5, Status = "available", Amenities = "WiFi,TV,Accessible Bathroom", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Room { Id = 10, RoomTypeId = 10, RoomNumber = 502, PricePerNight = 1000, Capacity = 4, BedType = "King", Size = "120 sq m", Floor = 5, Status = "available", Amenities = "WiFi,TV,Mini Bar,Balcony,City View,Private Pool", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        );
    }
}
