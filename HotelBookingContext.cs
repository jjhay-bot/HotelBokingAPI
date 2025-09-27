using Microsoft.EntityFrameworkCore;

public class HotelBookingContext : DbContext
{
    public HotelBookingContext(DbContextOptions<HotelBookingContext> options)
        : base(options)
    {
    }

    // DbSets for your entities will go here, e.g.:
    public DbSet<User> Users { get; set; }
    public DbSet<RoomType> RoomTypes { get; set; }
}
