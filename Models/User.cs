public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Role { get; set; } = "User"; // New property for user role
    public bool IsActive { get; set; } = true; // Controlled by admin: true = enabled, false = blocked/suspended
    public string PasswordHash { get; set; } = string.Empty; // For authentication
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public int? CurrentRoomId { get; set; } // Room currently occupied by user, null if none
    // Add navigation properties if needed later
}
