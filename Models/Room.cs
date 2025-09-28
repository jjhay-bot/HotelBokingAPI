public class Room
{
    public int Id { get; set; } // PK, auto-increment
    public int RoomTypeId { get; set; } // FK to RoomType
    public RoomType? RoomType { get; set; } // Navigation property
    public int RoomNumber { get; set; } // e.g., "101"
    public decimal PricePerNight { get; set; }
    public int Capacity { get; set; } // Number of guests the room can accommodate
    public string BedType { get; set; } = string.Empty; // e.g., "Queen", "King", "Twin"
    public string Size { get; set; } = string.Empty; // e.g., "30 sqm"
    public int Floor { get; set; } // 2nd Floor (2)
    public string Status { get; set; } = "Available"; // e.g., "Available", "Occupied", "Maintenance", "Reserved"
    public string Amenities { get; set; } = string.Empty; // e.g., "WiFi,TV,Mini Bar"
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public int? OccupiedByUserId { get; set; } // User currently occupying the room, null if vacant
}
