using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Api.Data;

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
        private readonly IMapper _mapper;

        // The constructor receives ApiContext via dependency injection.
        public UsersController(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: api/v1/users
        [HttpGet]
        [Authorize(Roles = "Admin")]
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
                "firstname" => desc ? query.OrderByDescending(u => u.FirstName) : query.OrderBy(u => u.FirstName),
                "lastname" => desc ? query.OrderByDescending(u => u.LastName) : query.OrderBy(u => u.LastName),
                "age" => desc ? query.OrderByDescending(u => u.Age) : query.OrderBy(u => u.Age),
                "email" => desc ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
                _ => desc ? query.OrderByDescending(u => u.Id) : query.OrderBy(u => u.Id)
            };

            // Pagination
            var totalCount = await query.CountAsync();
            var users = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var userResponses = _mapper.Map<List<UserDto>>(users);

            foreach (var userDto in userResponses)
            {
                var reservedBooking = await _context.Bookings
                    .Where(b => b.UserId == userDto.Id)
                    .OrderByDescending(b => b.CreatedAt)
                    .FirstOrDefaultAsync();
                if (reservedBooking != null)
                {
                    var room = await _context.Rooms.FindAsync(reservedBooking.RoomId);
                    userDto.ReservedRoomInfo = new ReservedRoomInfoDto
                    {
                        BookingId = reservedBooking.Id,
                        RoomId = reservedBooking.RoomId,
                        RoomNumber = room?.RoomNumber ?? 0,
                        Status = reservedBooking.Status,
                        StartDate = reservedBooking.StartDate,
                        EndDate = reservedBooking.EndDate
                    };
                }
            }

            var result = new
            {
                totalCount,
                page,
                pageSize,
                users = userResponses
            };

            return Ok(result);
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