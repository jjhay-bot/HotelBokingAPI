public class ReservedRoomInfoDto
{
    public int BookingId { get; set; }
    public int RoomId { get; set; }
    public int RoomNumber { get; set; } // Added room number
    public string? Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
