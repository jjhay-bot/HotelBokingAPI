using System;
using System.Collections.Generic;

public class RoomListItemDto
{
    public int Id { get; set; }
    public string RoomType { get; set; } = string.Empty; // e.g., "Luxury Suite"
    public string Description { get; set; } = string.Empty; // RoomType description
    public int RoomNumber { get; set; }
    public decimal PricePerNight { get; set; }
    public int Capacity { get; set; }
    public string BedType { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public int Floor { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<string> Amenities { get; set; } = new List<string>();
    public List<GalleryDto> Gallery { get; set; } = new List<GalleryDto>();
}
