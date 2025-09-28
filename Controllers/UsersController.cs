using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// This controller uses dependency injection to receive an instance of ApiContext.
// ApiContext is the Entity Framework Core DbContext for communicating with the database.
// All database operations for users will be performed through this context.

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class UsersController : ControllerBase
    {
        private readonly ApiContext _context;

        // The constructor receives ApiContext via dependency injection.
        public UsersController(ApiContext context)
        {
            _context = context;
        }
        // GET: api/v1/users
        [HttpGet]
        public async Task<IActionResult> GetUsers(
            [FromQuery] int? minAge,
            [FromQuery] int? maxAge,
            [FromQuery] string? sortBy = "Id",
            [FromQuery] bool desc = false,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _context.Users.AsQueryable();

            // Filtering
            if (minAge.HasValue)
                query = query.Where(u => u.Age >= minAge.Value);
            if (maxAge.HasValue)
                query = query.Where(u => u.Age <= maxAge.Value);

            // Sorting
            query = sortBy?.ToLower() switch
            {
                // Not indexed (no index by default - firstname/lastname)
                "firstname" => desc ? query.OrderByDescending(u => u.FirstName) : query.OrderBy(u => u.FirstName),
                "lastname" => desc ? query.OrderByDescending(u => u.LastName) : query.OrderBy(u => u.LastName),
                // Indexed column (uses index - efficient faster queries)
                "age" => desc ? query.OrderByDescending(u => u.Age) : query.OrderBy(u => u.Age),
                "email" => desc ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
                _ => desc ? query.OrderByDescending(u => u.Id) : query.OrderBy(u => u.Id)
            };

            // Pagination
            var users = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(users);
        }
        // POST: api/v1/users
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            // Trim spaces from email
            user.Email = user.Email.Trim();
            // Check if email already exists
            bool emailExists = await _context.Users.AnyAsync(u => u.Email == user.Email);
            if (emailExists)
            {
                return Conflict(new { message = "A user with this email already exists." });
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
        }
    }
}