using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/csrf-sessions")]
    public class CsrfController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult GetCsrfToken()
        {
            // Generate a random CSRF token
            var tokenBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(tokenBytes);
            }
            var csrfToken = Convert.ToBase64String(tokenBytes);

            // Set CSRF token in a non-HttpOnly, Secure cookie
            var jwtSettings = HttpContext.RequestServices.GetService<IConfiguration>()?.GetSection("Jwt");
            var expires = DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings?["ExpiresInMinutes"] ?? "60"));
            var cookieOptions = new CookieOptions
            {
                HttpOnly = false, // Must be readable by JS
                Secure = true,
                SameSite = SameSiteMode.None, // Allow cross-site cookie for FE/BE on separate domains
                Expires = expires
            };
            Response.Cookies.Append("XSRF-TOKEN", csrfToken, cookieOptions);

            // Return token in response body as well
            return Ok(new { csrfToken });
        }
    }
}
