public class Room
{
    public int Id { get; set; } // PK, auto-increment
    public int RoomTypeId { get; set; } // FK to RoomType
    public RoomType? RoomType { get; set; } // Navigation property
    public int RoomNumber { get; set; } // e.g., "101"
    public decimal PricePerNight { get; set; }
    public int Capacity { get; set; } // Number of guests the room can accommodate
    public string? BedType { get; set; } // e.g., "Queen", "King", "Twin"
    public string? Size { get; set; } // e.g., "30 sqm"
    public int Floor { get; set; } // 2nd Floor (2)
    public string? Status { get; set; } // e.g., "Available","Maintenance"
    public string? Amenities { get; set; } // e.g., "WiFi,TV,Mini Bar"
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int? OccupiedByUserId { get; set; } // User currently occupying the room, null if vacant
}
