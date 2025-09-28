namespace Api.DTOs
{
    public class RegisterRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Role { get; set; } // Optional, default to User
        public string? FirstName { get; set; } // Optional
        public string? LastName { get; set; } // Optional
        public int? Age { get; set; } // Optional
    }
}
