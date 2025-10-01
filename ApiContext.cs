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

        // Room Types
        modelBuilder.Entity<RoomType>().HasData(
            new RoomType { Id = 1, Name = "Luxury Suite", Description = "Spacious suite with a private balcony overlooking the city." },
            new RoomType { Id = 2, Name = "Standard Room", Description = "Comfortable room with all the basic amenities you need." },
            new RoomType { Id = 3, Name = "Family Room", Description = "Perfect for families, with two queen beds and a sofa." },
            new RoomType { Id = 4, Name = "Deluxe Room", Description = "An upgraded room with enhanced amenities and a comfortable seating area." },
            new RoomType { Id = 5, Name = "Penthouse Suite", Description = "The ultimate in luxury, featuring panoramic views, a private jacuzzi, and a full kitchen." },
            new RoomType { Id = 6, Name = "Studio Apartment", Description = "A modern, open-plan space with a small kitchenette, perfect for long-term stays." },
            new RoomType { Id = 7, Name = "Luxury Suite", Description = "Luxury suite with a garden view and exclusive lounge access." }
        );

        // Rooms
        modelBuilder.Entity<Room>().HasData(
            new Room { Id = 1, RoomTypeId = 1, RoomNumber = 101, PricePerNight = 250, Capacity = 2, BedType = "King Bed", Size = "65 sq m", Floor = 1, Status = "available", Amenities = "Balcony,King Bed,Mini Bar,City View,WiFi,Air Conditioning" },
            new Room { Id = 2, RoomTypeId = 2, RoomNumber = 102, PricePerNight = 120, Capacity = 2, BedType = "Queen Bed", Size = "35 sq m", Floor = 1, Status = "available", Amenities = "Queen Bed,WiFi,Flat-screen TV,Air Conditioning,Mini Fridge" },
            new Room { Id = 3, RoomTypeId = 3, RoomNumber = 201, PricePerNight = 180, Capacity = 4, BedType = "Two Queen Beds", Size = "50 sq m", Floor = 2, Status = "occupied", Amenities = "Two Queen Beds,Sofa,WiFi,TV,Air Conditioning,Mini Fridge" },
            new Room { Id = 4, RoomTypeId = 4, RoomNumber = 202, PricePerNight = 200, Capacity = 3, BedType = "King Bed", Size = "45 sq m", Floor = 2, Status = "available", Amenities = "King Bed,Lounge Area,Mini Bar,WiFi,Air Conditioning,Room Service" },
            new Room { Id = 5, RoomTypeId = 5, RoomNumber = 301, PricePerNight = 500, Capacity = 4, BedType = "King Bed + Sofa Bed", Size = "120 sq m", Floor = 3, Status = "maintenance", Amenities = "Jacuzzi,Full Kitchen,Panoramic View,Private Terrace,WiFi,Premium Sound System" },
            new Room { Id = 6, RoomTypeId = 6, RoomNumber = 302, PricePerNight = 220, Capacity = 2, BedType = "Queen Bed", Size = "40 sq m", Floor = 3, Status = "available", Amenities = "Kitchenette,Work Desk,WiFi,TV,Air Conditioning,Weekly Housekeeping" },
            new Room { Id = 7, RoomTypeId = 1, RoomNumber = 103, PricePerNight = 260, Capacity = 2, BedType = "King Bed", Size = "68 sq m", Floor = 1, Status = "available", Amenities = "Balcony,King Bed,Mini Bar,City View,WiFi,Air Conditioning" },
            new Room { Id = 8, RoomTypeId = 1, RoomNumber = 104, PricePerNight = 255, Capacity = 3, BedType = "King Bed", Size = "70 sq m", Floor = 1, Status = "available", Amenities = "Balcony,King Bed,Mini Bar,WiFi,Air Conditioning" },
            new Room { Id = 9, RoomTypeId = 4, RoomNumber = 203, PricePerNight = 210, Capacity = 3, BedType = "Queen Bed", Size = "48 sq m", Floor = 2, Status = "available", Amenities = "Queen Bed,Lounge Area,WiFi,Air Conditioning,Room Service" },
            new Room { Id = 10, RoomTypeId = 4, RoomNumber = 204, PricePerNight = 215, Capacity = 2, BedType = "King Bed", Size = "50 sq m", Floor = 2, Status = "available", Amenities = "King Bed,Lounge Area,WiFi,Air Conditioning,Room Service" },
            new Room { Id = 11, RoomTypeId = 5, RoomNumber = 302, PricePerNight = 520, Capacity = 4, BedType = "King Bed + Sofa Bed", Size = "125 sq m", Floor = 3, Status = "available", Amenities = "Jacuzzi,Full Kitchen,Panoramic View,Private Terrace,WiFi,Premium Sound System" },
            new Room { Id = 12, RoomTypeId = 5, RoomNumber = 303, PricePerNight = 530, Capacity = 5, BedType = "King Bed + Sofa Bed", Size = "130 sq m", Floor = 3, Status = "available", Amenities = "Jacuzzi,Full Kitchen,Panoramic View,Private Terrace,WiFi,Premium Sound System" }
        );

        // Gallery
        modelBuilder.Entity<Gallery>().HasData(
            // Room 1 Gallery
            new Gallery { Id = 1, RoomId = 1, Title = "Room Overview - Main Living Area", Img = "https://images.unsplash.com/photo-1631049307264-da0ec9d70304?q=80&w=1740&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Alt = "Luxury suite main living area with modern furnishing" },
            new Gallery { Id = 2, RoomId = 1, Title = "Bedroom View - King Size Bed", Img = "https://images.unsplash.com/photo-1618773928121-c32242e63f39?q=80&w=1740&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Alt = "King size bed with premium linens" },
            new Gallery { Id = 3, RoomId = 1, Title = "Bathroom - Modern Amenities", Img = "https://images.unsplash.com/photo-1584622781564-1d987f7333c1?q=80&w=1740&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Alt = "Modern bathroom with luxury amenities" },
            new Gallery { Id = 4, RoomId = 1, Title = "Balcony View - Ocean Terrace", Img = "https://images.unsplash.com/photo-1571896349842-33c89424de2d?q=80&w=1740&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Alt = "Private balcony with ocean view" },
            // Room 2 Gallery
            new Gallery { Id = 5, RoomId = 2, Title = "Room Overview - Cozy Standard Layout", Img = "https://images.unsplash.com/photo-1590490360182-c33d57733427?q=80&w=1674&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Alt = "Standard room with queen bed and modern furnishing" },
            new Gallery { Id = 6, RoomId = 2, Title = "Bedroom Area - Queen Size Comfort", Img = "https://images.unsplash.com/photo-1566665797739-1674de7a421a?q=80&w=1674&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Alt = "Queen bed with clean white linens" },
            new Gallery { Id = 7, RoomId = 2, Title = "Work Area - Desk & Chair Setup", Img = "https://images.unsplash.com/photo-1560472354-b33ff0c44a43?q=80&w=1626&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Alt = "Modern work desk with chair in hotel room" },
            new Gallery { Id = 8, RoomId = 2, Title = "Bathroom - Clean & Functional", Img = "https://images.unsplash.com/photo-1584622650111-993a426fbf0a?q=80&w=1740&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Alt = "Clean modern bathroom with shower" },
            // Room 3 Gallery
            new Gallery { Id = 9, RoomId = 3, Title = "Room Overview - Spacious Family Layout", Img = "https://images.unsplash.com/photo-1578683010236-d716f9a3f461?q=80&w=1740&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Alt = "Spacious family room with multiple beds" },
            new Gallery { Id = 10, RoomId = 3, Title = "Sleeping Area - Two Queen Beds", Img = "https://images.unsplash.com/photo-1602002418082-a4443e081dd1?q=80&w=1674&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Alt = "Two queen beds in family room" },
            new Gallery { Id = 11, RoomId = 3, Title = "Living Area - Comfortable Seating", Img = "https://images.unsplash.com/photo-1586023492125-27b2c045efd7?q=80&w=1658&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Alt = "Comfortable seating area with sofa" },
            new Gallery { Id = 12, RoomId = 3, Title = "Entertainment Center - TV & Storage", Img = "https://images.unsplash.com/photo-1551918120-9739cb430c6d?q=80&w=1587&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Alt = "TV entertainment center in hotel room" },
            // Room 4 Gallery
            new Gallery { Id = 13, RoomId = 4, Title = "Room Overview - Deluxe Comfort", Img = "https://images.unsplash.com/photo-1618773928121-c32242e63f39?q=80&w=1740&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Alt = "Deluxe room with modern decor and king bed" },
            new Gallery { Id = 14, RoomId = 4, Title = "King Bedroom - Premium Bedding", Img = "https://images.unsplash.com/photo-1522771739844-6a9f6d5f14af?q=80&w=1740&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Alt = "King bed with premium white linens" },
            new Gallery { Id = 15, RoomId = 4, Title = "Lounge Area - Relaxation Space", Img = "https://images.unsplash.com/photo-1564078516393-cf04bd966897?q=80&w=1587&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Alt = "Comfortable lounge seating area" },
            new Gallery { Id = 16, RoomId = 4, Title = "Mini Bar - Premium Refreshments", Img = "https://images.unsplash.com/photo-1582719478250-c89cae4dc85b?q=80&w=1740&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Alt = "Well-stocked mini bar area" },
            // Room 5 Gallery
            new Gallery { Id = 17, RoomId = 5, Title = "Suite Overview - Luxury Living Space", Img = "https://images.unsplash.com/photo-1590381105924-c72589b9ef3f?q=80&w=1739&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Alt = "Luxury penthouse suite living area" },
            new Gallery { Id = 18, RoomId = 5, Title = "Master Bedroom - King Size Elegance", Img = "https://images.unsplash.com/photo-1505693416388-ac5ce068fe85?q=80&w=1740&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Alt = "Elegant master bedroom with king bed" },
            new Gallery { Id = 19, RoomId = 5, Title = "Private Jacuzzi - Spa Experience", Img = "https://images.unsplash.com/photo-1571896349842-33c89424de2d?q=80&w=1740&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Alt = "Private jacuzzi with city views" },
            new Gallery { Id = 20, RoomId = 5, Title = "Full Kitchen - Gourmet Facilities", Img = "https://images.unsplash.com/photo-1556912173-3bb406ef7e77?q=80&w=1740&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Alt = "Modern full kitchen with premium appliances" },
            // Room 6 Gallery
            new Gallery { Id = 21, RoomId = 6, Title = "Studio Overview - Open Plan Living", Img = "https://images.unsplash.com/photo-1586023492125-27b2c045efd7?q=80&w=1658&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Alt = "Modern studio apartment with open plan design" },
            new Gallery { Id = 22, RoomId = 6, Title = "Sleeping Area - Queen Bed Setup", Img = "https://images.unsplash.com/photo-1560472354-b33ff0c44a43?q=80&w=1626&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Alt = "Queen bed area in studio apartment" },
            new Gallery { Id = 23, RoomId = 6, Title = "Kitchenette - Compact Cooking Space", Img = "https://images.unsplash.com/photo-1604709177225-055f99402ea3?q=80&w=1740&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Alt = "Compact kitchenette with modern appliances" },
            new Gallery { Id = 24, RoomId = 6, Title = "Work Space - Desk & Storage", Img = "https://images.unsplash.com/photo-1586023492125-27b2c045efd7?q=80&w=1658&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Alt = "Dedicated workspace with desk and storage" },
            // Room 7 Gallery
            new Gallery { Id = 25, RoomId = 7, Title = "Room Overview - Modern King Suite", Img = "https://images.unsplash.com/photo-1506744038136-46273834b3fb?q=80&w=1740&auto=format&fit=crop", Alt = "Modern king suite with city view and balcony" },
            // Room 8 Gallery
            new Gallery { Id = 26, RoomId = 8, Title = "Room Overview - Spacious King Room", Img = "https://images.unsplash.com/photo-1512918728675-ed5a9ecdebfd?q=80&w=1740&auto=format&fit=crop", Alt = "Spacious king room with balcony and modern decor" },
            // Room 9 Gallery
            new Gallery { Id = 27, RoomId = 9, Title = "Room Overview - Deluxe Queen Room", Img = "https://images.unsplash.com/photo-1464983953574-0892a716854b?q=80&w=1740&auto=format&fit=crop", Alt = "Deluxe queen room with lounge area" },
            // Room 10 Gallery
            new Gallery { Id = 28, RoomId = 10, Title = "Room Overview - King Deluxe", Img = "https://images.unsplash.com/photo-1507089947368-19c1da9775ae?q=80&w=1740&auto=format&fit=crop", Alt = "King deluxe room with lounge and modern amenities" },
            // Room 11 Gallery
            new Gallery { Id = 29, RoomId = 11, Title = "Penthouse Suite - Living Area", Img = "https://images.unsplash.com/photo-1519125323398-675f0ddb6308?q=80&w=1740&auto=format&fit=crop", Alt = "Penthouse suite living area with panoramic view" },
            // Room 12 Gallery
            new Gallery { Id = 30, RoomId = 12, Title = "Penthouse Suite - Master Bedroom", Img = "https://images.unsplash.com/photo-1519974719765-e6559eac2575?q=80&w=1740&auto=format&fit=crop", Alt = "Penthouse suite master bedroom with luxury furnishings" },
            // Room 7 Gallery (Unsplash)
            new Gallery { Id = 31, RoomId = 7, Title = "Modern King Suite - Living Area", Img = "https://plus.unsplash.com/premium_photo-1676321688630-9558e7d2be10?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=1080", Alt = "Modern king suite living area, Unsplash" },
            new Gallery { Id = 32, RoomId = 7, Title = "Modern King Suite - Bedroom", Img = "https://images.unsplash.com/photo-1605346434674-a440ca4dc4c0?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=1080", Alt = "King suite bedroom, Unsplash" },
            new Gallery { Id = 33, RoomId = 7, Title = "Modern King Suite - Window View", Img = "https://images.unsplash.com/photo-1559414059-34fe0a59e57a?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=1080", Alt = "King suite window view, Unsplash" },
            // Room 8 Gallery (Unsplash)
            new Gallery { Id = 34, RoomId = 8, Title = "Spacious King Room - Living Area", Img = "https://images.unsplash.com/photo-1609766857041-ed402ea8069a?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=1080", Alt = "Spacious king room living area, Unsplash" },
            new Gallery { Id = 35, RoomId = 8, Title = "Spacious King Room - Decor", Img = "https://plus.unsplash.com/premium_photo-1684175656161-21dddfef1324?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=1080", Alt = "King room decor, Unsplash" },
            new Gallery { Id = 36, RoomId = 8, Title = "Spacious King Room - Bed", Img = "https://images.unsplash.com/photo-1562438668-bcf0ca6578f0?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=1080", Alt = "King room bed, Unsplash" },
            // Room 9 Gallery (Unsplash)
            new Gallery { Id = 37, RoomId = 9, Title = "Deluxe Queen Room - Armchairs", Img = "https://images.unsplash.com/photo-1507038772120-7fff76f79d79?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=1080", Alt = "Deluxe queen room armchairs, Unsplash" },
            new Gallery { Id = 38, RoomId = 9, Title = "Deluxe Queen Room - Bed", Img = "https://images.unsplash.com/photo-1631049552240-59c37f38802b?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=1080", Alt = "Deluxe queen room bed, Unsplash" },
            new Gallery { Id = 39, RoomId = 9, Title = "Deluxe Queen Room - Nightstand", Img = "https://plus.unsplash.com/premium_photo-1671269704807-5479855d03fe?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=1080", Alt = "Deluxe queen room nightstand, Unsplash" },
            // Room 10 Gallery (Unsplash)
            new Gallery { Id = 40, RoomId = 10, Title = "King Deluxe - Living Area", Img = "https://images.unsplash.com/photo-1631048835388-46642de1582e?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=1080", Alt = "King deluxe living area, Unsplash" },
            new Gallery { Id = 41, RoomId = 10, Title = "King Deluxe - Bed", Img = "https://images.unsplash.com/photo-1595161695996-f746349f4945?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=1080", Alt = "King deluxe bed, Unsplash" },
            new Gallery { Id = 42, RoomId = 10, Title = "King Deluxe - Tray Service", Img = "https://plus.unsplash.com/premium_photo-1689609949898-5f7a10649fef?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=1080", Alt = "King deluxe tray service, Unsplash" },
            // Room 11 Gallery (Unsplash)
            new Gallery { Id = 43, RoomId = 11, Title = "Penthouse Suite - Dining Area", Img = "https://images.unsplash.com/photo-1621293954908-907159247fc8?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=1080", Alt = "Penthouse suite dining area, Unsplash" },
            new Gallery { Id = 44, RoomId = 11, Title = "Penthouse Suite - Floral Sofa", Img = "https://images.unsplash.com/photo-1592230228921-df3a88a2244e?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=1080", Alt = "Penthouse suite floral sofa, Unsplash" },
            new Gallery { Id = 45, RoomId = 11, Title = "Penthouse Suite - Pillows", Img = "https://images.unsplash.com/photo-1559841771-599b6eeaca62?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=1080", Alt = "Penthouse suite pillows, Unsplash" },
            // Room 12 Gallery (Unsplash)
            new Gallery { Id = 46, RoomId = 12, Title = "Penthouse Suite - Windows", Img = "https://plus.unsplash.com/premium_photo-1684445035187-c4bc7c96bc5d?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=1080", Alt = "Penthouse suite windows, Unsplash" },
            new Gallery { Id = 47, RoomId = 12, Title = "Penthouse Suite - Curtain View", Img = "https://images.unsplash.com/photo-1590490359854-dfba19688d70?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=1080", Alt = "Penthouse suite curtain view, Unsplash" },
            new Gallery { Id = 48, RoomId = 12, Title = "Penthouse Suite - Chair & Bed", Img = "https://images.unsplash.com/photo-1631049307290-bb947b114627?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=1080", Alt = "Penthouse suite chair and bed, Unsplash" }
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

        modelBuilder.Entity<Room>()
            .HasIndex(r => r.RoomTypeId);

        modelBuilder.Entity<Room>()
            .HasIndex(r => r.Capacity);

        modelBuilder.Entity<Room>()
            .HasIndex(r => r.PricePerNight);

        modelBuilder.Entity<Booking>()
            .HasIndex(b => new { b.RoomId, b.StartDate, b.EndDate });
    }
}
