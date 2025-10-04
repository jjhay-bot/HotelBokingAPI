using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IConfiguration _config;

        public AuthController(ApiContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }
            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }
            var token = GenerateJwtToken(user);

            // Generate refresh token
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7); // 7 days expiry
            await _context.SaveChangesAsync();

            // Set JWT as HttpOnly cookie
            var jwtSettings = _config.GetSection("Jwt");
            var secureCookie = Environment.GetEnvironmentVariable("COOKIE_SECURE") == "true";
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = secureCookie,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpiresInMinutes"]!))
            };
            Response.Cookies.Append("jwt", token, cookieOptions);

            // Set refresh token as HttpOnly cookie
            var refreshCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = secureCookie,
                SameSite = SameSiteMode.None,
                Expires = user.RefreshTokenExpiry
            };
            Response.Cookies.Append("refreshToken", refreshToken, refreshCookieOptions);

            return Ok(new {
                user = new {
                    id = user.Id,
                    email = user.Email,
                    role = user.Role,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    isActive = user.IsActive
                }
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized(new { message = "Refresh token missing." });
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                return Unauthorized(new { message = "Invalid or expired refresh token." });
            }
            // Optionally rotate refresh token
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(user);
            var jwtSettings = _config.GetSection("Jwt");
            var secureCookie = Environment.GetEnvironmentVariable("COOKIE_SECURE") == "true";
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = secureCookie,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpiresInMinutes"]!))
            };
            Response.Cookies.Append("jwt", token, cookieOptions);

            var refreshCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = secureCookie,
                SameSite = SameSiteMode.None,
                Expires = user.RefreshTokenExpiry
            };
            Response.Cookies.Append("refreshToken", newRefreshToken, refreshCookieOptions);

            return Ok(new { message = "Token refreshed." });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (!string.IsNullOrEmpty(refreshToken))
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
                if (user != null)
                {
                    user.RefreshToken = null;
                    user.RefreshTokenExpiry = null;
                    await _context.SaveChangesAsync();
                }
            }
            var secureCookie = Environment.GetEnvironmentVariable("COOKIE_SECURE") == "true";
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = secureCookie,
                SameSite = SameSiteMode.None,
                Path = "/"
            };
            Response.Cookies.Delete("jwt", cookieOptions);
            Response.Cookies.Delete("refreshToken", cookieOptions);
            return Ok(new { message = "Logged out successfully." });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            // Get userId from JWT claims
            var userIdClaim = User.FindFirst("userId");
            if (userIdClaim == null)
            {
                return Unauthorized(new { message = "User not authenticated." });
            }
            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized(new { message = "Invalid user id in token." });
            }
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return Unauthorized(new { message = "User not found." });
            }
            return Ok(new {
                id = user.Id,
                email = user.Email,
                role = user.Role,
                firstName = user.FirstName,
                lastName = user.LastName,
                age = user.Age,
                isActive = user.IsActive
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            var email = dto.Email.Trim();
            bool emailExists = await _context.Users.AnyAsync(u => u.Email == email);
            if (emailExists)
            {
                return Conflict(new { message = "A user with this email already exists." });
            }
            var hasher = new PasswordHasher<User>();
            var user = new User
            {
                Email = email,
                Role = string.IsNullOrEmpty(dto.Role) ? "User" : dto.Role,
                FirstName = dto.FirstName ?? string.Empty,
                LastName = dto.LastName ?? string.Empty,
                Age = dto.Age ?? 0,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            user.PasswordHash = hasher.HashPassword(user, dto.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Register), new { id = user.Id }, user);
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpiresInMinutes"]!));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role ?? "User"),
                new Claim("firstName", user.FirstName ?? string.Empty),
                new Claim("lastName", user.LastName ?? string.Empty),
                new Claim("age", user.Age.ToString()),
                new Claim("userId", user.Id.ToString()) // Add userId to claims
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }
    }
}
