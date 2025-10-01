using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Controllers
{
    [ApiController]
    [Route("api/v1/bookings")]
    public class BookingsController : ControllerBase
    {
        private readonly ApiContext _context;
        public BookingsController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                .AsQueryable();

            var totalCount = await query.CountAsync();
            var bookings = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new
            {
                totalCount,
                page,
                pageSize,
                bookings
            };
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (booking == null) return NotFound();
            return Ok(booking);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] Booking booking)
        {
            // Extract userId from claims
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "User ID not found in token." });
            }
            booking.UserId = userId; // Always use userId from claims

            // Prevent overlapping bookings for Reserved or Confirmed status
            if (booking.Status == "Reserved" || booking.Status == "Confirmed")
            {
                bool hasConflict = await _context.Bookings.AnyAsync(b =>
                    b.RoomId == booking.RoomId &&
                    (b.Status == "Reserved" || b.Status == "Confirmed") &&
                    b.StartDate < booking.EndDate && booking.StartDate < b.EndDate
                );
                if (hasConflict)
                {
                    return Conflict(new { message = "Room is already booked or reserved for the selected dates." });
                }
            }

            _context.Bookings.Add(booking);
            var room = await _context.Rooms.FindAsync(booking.RoomId);
            var user = await _context.Users.FindAsync(booking.UserId);
            var now = System.DateTime.UtcNow;
            if (room != null)
            {
                if (booking.Status == "Confirmed" && booking.StartDate <= now && booking.EndDate > now)
                {
                    room.Status = "Occupied";
                    room.OccupiedByUserId = booking.UserId;
                }
                else if (booking.Status == "Reserved" && booking.StartDate > now)
                {
                    // Only set to Reserved if not already Occupied
                    if (room.Status != "Occupied")
                        room.Status = "Reserved";
                }
                room.UpdatedAt = now;
            }
            if (user != null && booking.Status == "Confirmed")
            {
                user.CurrentRoomId = booking.RoomId;
            }
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = booking.Id }, booking);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] Booking booking)
        {
            if (id != booking.Id) return BadRequest();
            // Get userId from claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null) return Unauthorized();
            if (!int.TryParse(userIdClaim.Value, out int userId)) return Unauthorized();
            booking.UserId = userId;
            var existing = await _context.Bookings.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
            _context.Entry(booking).State = EntityState.Modified;
            var room = await _context.Rooms.FindAsync(booking.RoomId);
            var user = await _context.Users.FindAsync(booking.UserId);
            var now = System.DateTime.UtcNow;
            // Always set CurrentRoomId for active confirmed bookings
            if (booking.Status == "Confirmed" && booking.StartDate <= now && booking.EndDate > now)
            {
                if (room != null)
                {
                    room.Status = "Occupied";
                    room.OccupiedByUserId = booking.UserId;
                    room.UpdatedAt = now;
                }
                if (user != null)
                {
                    user.CurrentRoomId = booking.RoomId;
                }
            }
            // If status is changed to CheckedOut, reset room and user
            if (booking.Status == "CheckedOut" && existing != null && existing.Status != "CheckedOut")
            {
                if (room != null)
                {
                    // Check if there are any future Reserved/Confirmed bookings for this room
                    bool hasFuture = await _context.Bookings.AnyAsync(b =>
                        b.RoomId == booking.RoomId &&
                        (b.Status == "Reserved" || b.Status == "Confirmed") &&
                        b.StartDate > now
                    );
                    room.Status = hasFuture ? "Reserved" : "Available";
                    room.OccupiedByUserId = null;
                    room.UpdatedAt = now;
                }
                if (user != null)
                {
                    user.CurrentRoomId = null;
                }
            }
            // If status is Reserved and future, set room status to Reserved if not Occupied
            if (booking.Status == "Reserved" && booking.StartDate > now)
            {
                if (room != null && room.Status != "Occupied")
                {
                    room.Status = "Reserved";
                    room.UpdatedAt = now;
                }
            }
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> Patch(int id, [FromBody] Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<Booking> patchDoc)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();
            var oldStatus = booking.Status;
            patchDoc.ApplyTo(booking, (Microsoft.AspNetCore.JsonPatch.JsonPatchError error) =>
            {
                ModelState.AddModelError(error.AffectedObject?.ToString() ?? "", error.ErrorMessage ?? "Patch error");
            });
            if (!ModelState.IsValid) return BadRequest(ModelState);
            booking.UpdatedAt = System.DateTime.UtcNow;
            var room = await _context.Rooms.FindAsync(booking.RoomId);
            var user = await _context.Users.FindAsync(booking.UserId);
            var now = System.DateTime.UtcNow;
            // Always set CurrentRoomId for active confirmed bookings
            if (booking.Status == "Confirmed" && booking.StartDate <= now && booking.EndDate > now)
            {
                if (room != null)
                {
                    room.Status = "Occupied";
                    room.OccupiedByUserId = booking.UserId;
                    room.UpdatedAt = now;
                }
                if (user != null)
                {
                    user.CurrentRoomId = booking.RoomId;
                }
            }
            // If status is changed to CheckedOut, reset room and user
            if (booking.Status == "CheckedOut" && oldStatus != "CheckedOut")
            {
                if (room != null)
                {
                    // Check if there are any future Reserved/Confirmed bookings for this room
                    bool hasFuture = await _context.Bookings.AnyAsync(b =>
                        b.RoomId == booking.RoomId &&
                        (b.Status == "Reserved" || b.Status == "Confirmed") &&
                        b.StartDate > now
                    );
                    room.Status = hasFuture ? "Reserved" : "Available";
                    room.OccupiedByUserId = null;
                    room.UpdatedAt = now;
                }
                if (user != null)
                {
                    user.CurrentRoomId = null;
                }
            }
            // If status is Reserved and future, set room status to Reserved if not Occupied
            if (booking.Status == "Reserved" && booking.StartDate > now)
            {
                if (room != null && room.Status != "Occupied")
                {
                    room.Status = "Reserved";
                    room.UpdatedAt = now;
                }
            }
            await _context.SaveChangesAsync();
            return Ok(booking);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var bookings = await _context.Bookings.Where(b => b.UserId == userId).ToListAsync();
            return Ok(bookings);
        }
    }
}
