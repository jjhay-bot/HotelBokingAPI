using System;
using System.Collections.Generic;

public class RoomDto
{
    public int Id { get; set; }
    public int RoomTypeId { get; set; }
    public int RoomNumber { get; set; }
    public decimal PricePerNight { get; set; }
    public int Capacity { get; set; }
    public string BedType { get; set; }
    public string Size { get; set; }
    public int Floor { get; set; }
    public string Status { get; set; }
    public List<string> Amenities { get; set; } = new List<string>();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int? OccupiedByUserId { get; set; } // User currently occupying the room, null if vacant
}
