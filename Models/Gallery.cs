public class Gallery
{
    public int Id { get; set; }
    public int RoomId { get; set; } // Foreign key to Room
    public string Title { get; set; } = string.Empty;
    public string Img { get; set; } = string.Empty;
    public string Alt { get; set; } = string.Empty;
    public Room? Room { get; set; } // Navigation property
}
