public class RoomType
{
    public int Id { get; set; } // PK, auto-increment
    public string Name { get; set; } = string.Empty; // e.g., "Single"
    public string? Description { get; set; } // optional
    // Navigation property for related rooms (optional, for later)
    // public ICollection<Room> Rooms { get; set; }
}
