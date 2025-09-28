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
    public DbSet<Gallery> Galleries { get; set; }
    public DbSet<Booking> Bookings { get; set; }

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
            new Room { Id = 1, RoomTypeId = 1, RoomNumber = 101, PricePerNight = 100, Capacity = 1, BedType = "Single", Size = "20 sq m", Floor = 1, Status = "available", Amenities = "WiFi,TV,Desk", CreatedAt = new DateTime(2025, 9, 28, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 9, 28, 0, 0, 0, DateTimeKind.Utc) },
            new Room { Id = 2, RoomTypeId = 2, RoomNumber = 102, PricePerNight = 150, Capacity = 2, BedType = "Double", Size = "30 sq m", Floor = 1, Status = "available", Amenities = "WiFi,TV,Mini Bar", CreatedAt = new DateTime(2025, 9, 28, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 9, 28, 0, 0, 0, DateTimeKind.Utc) },
            new Room { Id = 3, RoomTypeId = 3, RoomNumber = 201, PricePerNight = 200, Capacity = 2, BedType = "Queen", Size = "35 sq m", Floor = 2, Status = "occupied", Amenities = "WiFi,TV,Balcony", CreatedAt = new DateTime(2025, 9, 28, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 9, 28, 0, 0, 0, DateTimeKind.Utc) },
            new Room { Id = 4, RoomTypeId = 4, RoomNumber = 202, PricePerNight = 250, Capacity = 2, BedType = "King", Size = "40 sq m", Floor = 2, Status = "maintenance", Amenities = "WiFi,TV,Mini Bar,Balcony", CreatedAt = new DateTime(2025, 9, 28, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 9, 28, 0, 0, 0, DateTimeKind.Utc) },
            new Room { Id = 5, RoomTypeId = 5, RoomNumber = 301, PricePerNight = 180, Capacity = 4, BedType = "Double", Size = "45 sq m", Floor = 3, Status = "available", Amenities = "WiFi,TV,Kitchenette", CreatedAt = new DateTime(2025, 9, 28, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 9, 28, 0, 0, 0, DateTimeKind.Utc) },
            new Room { Id = 6, RoomTypeId = 6, RoomNumber = 302, PricePerNight = 500, Capacity = 2, BedType = "King", Size = "80 sq m", Floor = 3, Status = "available", Amenities = "WiFi,TV,Mini Bar,City View", CreatedAt = new DateTime(2025, 9, 28, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 9, 28, 0, 0, 0, DateTimeKind.Utc) },
            new Room { Id = 7, RoomTypeId = 7, RoomNumber = 401, PricePerNight = 120, Capacity = 1, BedType = "Single", Size = "25 sq m", Floor = 4, Status = "available", Amenities = "WiFi,TV,Desk", CreatedAt = new DateTime(2025, 9, 28, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 9, 28, 0, 0, 0, DateTimeKind.Utc) },
            new Room { Id = 8, RoomTypeId = 8, RoomNumber = 402, PricePerNight = 300, Capacity = 2, BedType = "King", Size = "60 sq m", Floor = 4, Status = "available", Amenities = "WiFi,TV,Mini Bar,Balcony,City View", CreatedAt = new DateTime(2025, 9, 28, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 9, 28, 0, 0, 0, DateTimeKind.Utc) },
            new Room { Id = 9, RoomTypeId = 9, RoomNumber = 501, PricePerNight = 110, Capacity = 2, BedType = "Twin", Size = "28 sq m", Floor = 5, Status = "available", Amenities = "WiFi,TV,Accessible Bathroom", CreatedAt = new DateTime(2025, 9, 28, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 9, 28, 0, 0, 0, DateTimeKind.Utc) },
            new Room { Id = 10, RoomTypeId = 10, RoomNumber = 502, PricePerNight = 1000, Capacity = 4, BedType = "King", Size = "120 sq m", Floor = 5, Status = "available", Amenities = "WiFi,TV,Mini Bar,Balcony,City View,Private Pool", CreatedAt = new DateTime(2025, 9, 28, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 9, 28, 0, 0, 0, DateTimeKind.Utc) }
        );

        modelBuilder.Entity<Gallery>().HasData(
            // Room 1
            new Gallery { Id = 1, RoomId = 1, Title = "Room 1 - Main View", Img = "https://images.unsplash.com/photo-1506744038136-46273834b3fb?auto=format&fit=crop&w=800&q=80", Alt = "Modern single hotel room with large window" },
            new Gallery { Id = 2, RoomId = 1, Title = "Room 1 - Bed Area", Img = "https://images.unsplash.com/photo-1552901467-1c1c7b2b1b8b?auto=format&fit=crop&w=800&q=80", Alt = "Single bed with white linens and accent wall" },
            new Gallery { Id = 3, RoomId = 1, Title = "Room 1 - Bathroom", Img = "https://images.unsplash.com/photo-1507089947368-19c1da9775ae?auto=format&fit=crop&w=800&q=80", Alt = "Modern bathroom with glass shower" },
            new Gallery { Id = 4, RoomId = 1, Title = "Room 1 - Desk", Img = "https://images.unsplash.com/photo-1515378791036-0648a3ef77b2?auto=format&fit=crop&w=800&q=80", Alt = "Work desk in hotel room" },
            // Room 2
            new Gallery { Id = 5, RoomId = 2, Title = "Room 2 - Main View", Img = "https://images.unsplash.com/photo-1512918728675-ed5a9ecdebfd?auto=format&fit=crop&w=800&q=80", Alt = "Double room with two beds and city view" },
            new Gallery { Id = 6, RoomId = 2, Title = "Room 2 - Bed Area", Img = "https://images.unsplash.com/photo-1464983953574-0892a716854b?auto=format&fit=crop&w=800&q=80", Alt = "Double bed with modern decor" },
            new Gallery { Id = 7, RoomId = 2, Title = "Room 2 - Bathroom", Img = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?auto=format&fit=crop&w=800&q=80", Alt = "Bathroom with marble sink" },
            new Gallery { Id = 8, RoomId = 2, Title = "Room 2 - Mini Bar", Img = "https://images.unsplash.com/photo-1519125323398-675f0ddb6308?auto=format&fit=crop&w=800&q=80", Alt = "Mini bar and coffee station" },
            // Room 3
            new Gallery { Id = 9, RoomId = 3, Title = "Room 3 - Main View", Img = "https://images.unsplash.com/photo-1465101178521-c1a9136a3b99?auto=format&fit=crop&w=800&q=80", Alt = "Suite with living area and sofa" },
            new Gallery { Id = 10, RoomId = 3, Title = "Room 3 - Bed Area", Img = "https://images.unsplash.com/photo-1519710164239-da123dc03ef4?auto=format&fit=crop&w=800&q=80", Alt = "Queen bed in suite" },
            new Gallery { Id = 11, RoomId = 3, Title = "Room 3 - Bathroom", Img = "https://images.unsplash.com/photo-1465101046530-73398c7f28ca?auto=format&fit=crop&w=800&q=80", Alt = "Suite bathroom with tub" },
            new Gallery { Id = 12, RoomId = 3, Title = "Room 3 - Balcony", Img = "https://images.unsplash.com/photo-1506744038136-46273834b3fb?auto=format&fit=crop&w=800&q=80", Alt = "Suite balcony with city view" },
            // Room 4
            new Gallery { Id = 13, RoomId = 4, Title = "Room 4 - Main View", Img = "https://images.unsplash.com/photo-1503676382389-4809596d5290?auto=format&fit=crop&w=800&q=80", Alt = "Deluxe room with king bed and lounge area" },
            new Gallery { Id = 14, RoomId = 4, Title = "Room 4 - Bed Area", Img = "https://images.unsplash.com/photo-1519125323398-675f0ddb6308?auto=format&fit=crop&w=800&q=80", Alt = "King bed in deluxe room" },
            new Gallery { Id = 15, RoomId = 4, Title = "Room 4 - Bathroom", Img = "https://images.unsplash.com/photo-1465101046530-73398c7f28ca?auto=format&fit=crop&w=800&q=80", Alt = "Deluxe bathroom with rain shower" },
            new Gallery { Id = 16, RoomId = 4, Title = "Room 4 - Balcony", Img = "https://images.unsplash.com/photo-1464983953574-0892a716854b?auto=format&fit=crop&w=800&q=80", Alt = "Deluxe balcony with seating" },
            // Room 5
            new Gallery { Id = 17, RoomId = 5, Title = "Room 5 - Main View", Img = "https://images.unsplash.com/photo-1506744038136-46273834b3fb?auto=format&fit=crop&w=800&q=80", Alt = "Family room with multiple beds" },
            new Gallery { Id = 18, RoomId = 5, Title = "Room 5 - Bed Area", Img = "https://images.unsplash.com/photo-1512918728675-ed5a9ecdebfd?auto=format&fit=crop&w=800&q=80", Alt = "Double beds in family room" },
            new Gallery { Id = 19, RoomId = 5, Title = "Room 5 - Bathroom", Img = "https://images.unsplash.com/photo-1464983953574-0892a716854b?auto=format&fit=crop&w=800&q=80", Alt = "Family bathroom with tub" },
            new Gallery { Id = 20, RoomId = 5, Title = "Room 5 - Kitchenette", Img = "https://images.unsplash.com/photo-1500534314209-a25ddb2bd429?auto=format&fit=crop&w=800&q=80", Alt = "Family room kitchenette" },
            // Room 6
            new Gallery { Id = 21, RoomId = 6, Title = "Room 6 - Main View", Img = "https://images.unsplash.com/photo-1503676382389-4809596d5290?auto=format&fit=crop&w=800&q=80", Alt = "Penthouse with panoramic view" },
            new Gallery { Id = 22, RoomId = 6, Title = "Room 6 - Bed Area", Img = "https://images.unsplash.com/photo-1519125323398-675f0ddb6308?auto=format&fit=crop&w=800&q=80", Alt = "King bed in penthouse" },
            new Gallery { Id = 23, RoomId = 6, Title = "Room 6 - Bathroom", Img = "https://images.unsplash.com/photo-1465101046530-73398c7f28ca?auto=format&fit=crop&w=800&q=80", Alt = "Penthouse bathroom with luxury amenities" },
            new Gallery { Id = 24, RoomId = 6, Title = "Room 6 - City View", Img = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?auto=format&fit=crop&w=800&q=80", Alt = "Penthouse city view" },
            // Room 7
            new Gallery { Id = 25, RoomId = 7, Title = "Room 7 - Main View", Img = "https://images.unsplash.com/photo-1465101178521-c1a9136a3b99?auto=format&fit=crop&w=800&q=80", Alt = "Studio with workspace" },
            new Gallery { Id = 26, RoomId = 7, Title = "Room 7 - Bed Area", Img = "https://images.unsplash.com/photo-1519710164239-da123dc03ef4?auto=format&fit=crop&w=800&q=80", Alt = "Studio bed area" },
            new Gallery { Id = 27, RoomId = 7, Title = "Room 7 - Bathroom", Img = "https://images.unsplash.com/photo-1465101046530-73398c7f28ca?auto=format&fit=crop&w=800&q=80", Alt = "Studio bathroom" },
            new Gallery { Id = 28, RoomId = 7, Title = "Room 7 - Desk", Img = "https://images.unsplash.com/photo-1515378791036-0648a3ef77b2?auto=format&fit=crop&w=800&q=80", Alt = "Studio desk area" },
            // Room 8
            new Gallery { Id = 29, RoomId = 8, Title = "Room 8 - Main View", Img = "https://images.unsplash.com/photo-1503676382389-4809596d5290?auto=format&fit=crop&w=800&q=80", Alt = "Executive room with lounge" },
            new Gallery { Id = 30, RoomId = 8, Title = "Room 8 - Bed Area", Img = "https://images.unsplash.com/photo-1519125323398-675f0ddb6308?auto=format&fit=crop&w=800&q=80", Alt = "Executive bed area" },
            new Gallery { Id = 31, RoomId = 8, Title = "Room 8 - Bathroom", Img = "https://images.unsplash.com/photo-1465101046530-73398c7f28ca?auto=format&fit=crop&w=800&q=80", Alt = "Executive bathroom" },
            new Gallery { Id = 32, RoomId = 8, Title = "Room 8 - Balcony", Img = "https://images.unsplash.com/photo-1464983953574-0892a716854b?auto=format&fit=crop&w=800&q=80", Alt = "Executive balcony" },
            // Room 9
            new Gallery { Id = 33, RoomId = 9, Title = "Room 9 - Main View", Img = "https://images.unsplash.com/photo-1506744038136-46273834b3fb?auto=format&fit=crop&w=800&q=80", Alt = "Accessible room with modern amenities" },
            new Gallery { Id = 34, RoomId = 9, Title = "Room 9 - Bed Area", Img = "https://images.unsplash.com/photo-1512918728675-ed5a9ecdebfd?auto=format&fit=crop&w=800&q=80", Alt = "Accessible bed area" },
            new Gallery { Id = 35, RoomId = 9, Title = "Room 9 - Bathroom", Img = "https://images.unsplash.com/photo-1464983953574-0892a716854b?auto=format&fit=crop&w=800&q=80", Alt = "Accessible bathroom" },
            new Gallery { Id = 36, RoomId = 9, Title = "Room 9 - Accessible Bathroom", Img = "https://images.unsplash.com/photo-1515378791036-0648a3ef77b2?auto=format&fit=crop&w=800&q=80", Alt = "Accessible bathroom with support bars" },
            // Room 10
            new Gallery { Id = 37, RoomId = 10, Title = "Room 10 - Main View", Img = "https://images.unsplash.com/photo-1503676382389-4809596d5290?auto=format&fit=crop&w=800&q=80", Alt = "Presidential suite with luxury furnishings" },
            new Gallery { Id = 38, RoomId = 10, Title = "Room 10 - Bed Area", Img = "https://images.unsplash.com/photo-1519125323398-675f0ddb6308?auto=format&fit=crop&w=800&q=80", Alt = "Presidential suite bed area" },
            new Gallery { Id = 39, RoomId = 10, Title = "Room 10 - Bathroom", Img = "https://images.unsplash.com/photo-1465101046530-73398c7f28ca?auto=format&fit=crop&w=800&q=80", Alt = "Presidential suite bathroom" },
            new Gallery { Id = 40, RoomId = 10, Title = "Room 10 - Private Pool", Img = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?auto=format&fit=crop&w=800&q=80", Alt = "Presidential suite private pool" }
        );

        modelBuilder.Entity<Booking>()
            .HasIndex(b => b.UserId);
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.User)
            .WithMany()
            .HasForeignKey(b => b.UserId);
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Room)
            .WithMany()
            .HasForeignKey(b => b.RoomId);
    }
}
