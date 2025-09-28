public class Gallery
{
    public int Id { get; set; }
    public int RoomId { get; set; } // Foreign key to Room
    public string? Title { get; set; }
    public string? Img { get; set; }
    public string? Alt { get; set; }
    public Room? Room { get; set; } // Navigation property
}
