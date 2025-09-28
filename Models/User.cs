public class User
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public int Age { get; set; }
    public string? Role { get; set; } // New property for user role
    public bool IsActive { get; set; } // Controlled by admin: true = enabled, false = blocked/suspended
    public string? PasswordHash { get; set; } // For authentication
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? CurrentRoomId { get; set; } // Room currently occupied by user, null if none
    // Add navigation properties if needed later
}
