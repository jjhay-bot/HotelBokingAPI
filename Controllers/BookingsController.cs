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
        public async Task<IActionResult> GetAll()
        {
            var bookings = await _context.Bookings.ToListAsync();
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();
            return Ok(booking);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] Booking booking)
        {
            _context.Bookings.Add(booking);
            // If booking is confirmed, set room status to Occupied and tag user/room
            if (booking.Status == "Confirmed")
            {
                var room = await _context.Rooms.FindAsync(booking.RoomId);
                var user = await _context.Users.FindAsync(booking.UserId);
                if (room != null)
                {
                    room.Status = "Occupied";
                    room.OccupiedByUserId = booking.UserId;
                    room.UpdatedAt = System.DateTime.UtcNow;
                }
                if (user != null)
                {
                    user.CurrentRoomId = booking.RoomId;
                }
            }
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = booking.Id }, booking);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] Booking booking)
        {
            if (id != booking.Id) return BadRequest();
            var existing = await _context.Bookings.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
            _context.Entry(booking).State = EntityState.Modified;
            // Only update room/user status if status is newly set to Confirmed
            if (booking.Status == "Confirmed" && existing != null && existing.Status != "Confirmed")
            {
                var room = await _context.Rooms.FindAsync(booking.RoomId);
                var user = await _context.Users.FindAsync(booking.UserId);
                if (room != null)
                {
                    room.Status = "Occupied";
                    room.OccupiedByUserId = booking.UserId;
                    room.UpdatedAt = System.DateTime.UtcNow;
                }
                if (user != null)
                {
                    user.CurrentRoomId = booking.RoomId;
                }
            }
            // If status is changed to CheckedOut, reset room and user
            if (booking.Status == "CheckedOut" && existing != null && existing.Status != "CheckedOut")
            {
                var room = await _context.Rooms.FindAsync(booking.RoomId);
                var user = await _context.Users.FindAsync(booking.UserId);
                if (room != null)
                {
                    room.Status = "Available";
                    room.OccupiedByUserId = null;
                    room.UpdatedAt = System.DateTime.UtcNow;
                }
                if (user != null)
                {
                    user.CurrentRoomId = null;
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
            // Only update room/user status if status is newly set to Confirmed
            if (booking.Status == "Confirmed" && oldStatus != "Confirmed")
            {
                var room = await _context.Rooms.FindAsync(booking.RoomId);
                var user = await _context.Users.FindAsync(booking.UserId);
                if (room != null)
                {
                    room.Status = "Occupied";
                    room.OccupiedByUserId = booking.UserId;
                    room.UpdatedAt = System.DateTime.UtcNow;
                }
                if (user != null)
                {
                    user.CurrentRoomId = booking.RoomId;
                }
            }
            // If status is changed to CheckedOut, reset room and user
            if (booking.Status == "CheckedOut" && oldStatus != "CheckedOut")
            {
                var room = await _context.Rooms.FindAsync(booking.RoomId);
                var user = await _context.Users.FindAsync(booking.UserId);
                if (room != null)
                {
                    room.Status = "Available";
                    room.OccupiedByUserId = null;
                    room.UpdatedAt = System.DateTime.UtcNow;
                }
                if (user != null)
                {
                    user.CurrentRoomId = null;
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
