public class UserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Age { get; set; }
    public int? CurrentRoomId { get; set; }
    public ReservedRoomInfoDto? ReservedRoomInfo { get; set; } // Renamed for reserved room info
}
