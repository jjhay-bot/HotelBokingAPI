using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{

    [ApiController]
    [Route("api/v1/rooms")]
    public class RoomsController : ControllerBase
    {
        private readonly ApiContext _context;
        public RoomsController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetRooms(
            [FromQuery] int? roomTypeId = null,
            [FromQuery] int? guestCount = null,
            [FromQuery] DateTime? checkIn = null,
            [FromQuery] DateTime? checkOut = null,
            [FromQuery] string? status = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 2)
        {
            var roomsQuery = _context.Rooms
                .Include(r => r.RoomType)
                .OrderByDescending(r => r.Id)
                .AsQueryable(); // Start with all rooms, newest first

            if (roomTypeId.HasValue)
                roomsQuery = roomsQuery.Where(r => r.RoomTypeId == roomTypeId.Value); // Filter by room type if provided
            if (guestCount.HasValue)
                roomsQuery = roomsQuery.Where(r => r.Capacity >= guestCount.Value); // Filter by guest count if provided
            if (!string.IsNullOrEmpty(status))
                roomsQuery = roomsQuery.Where(r => r.Status == status); // Filter by status if provided

            // Filter by availability if both checkIn and checkOut are provided
            if (checkIn.HasValue && checkOut.HasValue)
            {
                var bookings = _context.Bookings
                    .Where(b =>
                        b.StartDate < checkOut.Value && b.EndDate > checkIn.Value
                    ); // Get bookings that overlap with the requested dates
                var bookedRoomIds = await bookings.Select(b => b.RoomId).Distinct().ToListAsync();
                roomsQuery = roomsQuery.Where(r => !bookedRoomIds.Contains(r.Id));
            }

            var totalCount = await roomsQuery.CountAsync();
            var rooms = await roomsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            var galleries = await _context.Galleries.ToListAsync();

            var roomDtos = rooms.Select(r => new RoomListItemDto
            {
                Id = r.Id,
                RoomType = r.RoomType?.Name ?? string.Empty,
                Description = r.RoomType?.Description ?? string.Empty,
                RoomNumber = r.RoomNumber,
                PricePerNight = r.PricePerNight,
                Capacity = r.Capacity,
                BedType = r.BedType ?? string.Empty,
                Size = r.Size ?? string.Empty,
                Floor = r.Floor,
                Status = r.Status ?? string.Empty,
                Amenities = string.IsNullOrEmpty(r.Amenities)
                    ? new List<string>()
                    : r.Amenities.Split(',').Select(a => a.Trim()).ToList(),
                Gallery = galleries
                    .Where(g => g.RoomId == r.Id)
                    .Select(g => new GalleryDto
                    {
                        Id = g.Id,
                        Title = g.Title ?? string.Empty,
                        Img = g.Img ?? string.Empty,
                        Alt = g.Alt ?? string.Empty
                    }).ToList()
            }).ToList();

            var result = new {
                totalCount,
                page,
                pageSize,
                rooms = roomDtos
            };
            return Ok(result);
        }

        // POST: api/v1/roomtypes
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRoom([FromBody] Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRooms), new { id = room.Id }, room);
        }

        // PATCH: api/v1/roomtypes/{id}
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PatchRoom(int id, [FromBody] JsonElement patchDoc)
        {
            var existing = await _context.Rooms.FindAsync(id);
            if (existing == null) return NotFound();

            // Only update provided fields
            if (patchDoc.TryGetProperty("roomTypeId", out var roomTypeIdProp))
                existing.RoomTypeId = roomTypeIdProp.GetInt32();
            if (patchDoc.TryGetProperty("roomNumber", out var roomNumberProp))
                existing.RoomNumber = roomNumberProp.GetInt32();
            if (patchDoc.TryGetProperty("pricePerNight", out var pricePerNightProp))
                existing.PricePerNight = pricePerNightProp.GetDecimal();
            if (patchDoc.TryGetProperty("capacity", out var capacityProp))
                existing.Capacity = capacityProp.GetInt32();
            if (patchDoc.TryGetProperty("bedType", out var bedTypeProp))
                existing.BedType = bedTypeProp.GetString() ?? existing.BedType;
            if (patchDoc.TryGetProperty("size", out var sizeProp))
                existing.Size = sizeProp.GetString() ?? existing.Size;
            if (patchDoc.TryGetProperty("floor", out var floorProp))
                existing.Floor = floorProp.GetInt32();
            if (patchDoc.TryGetProperty("status", out var statusProp))
                existing.Status = statusProp.GetString() ?? existing.Status;
            if (patchDoc.TryGetProperty("amenities", out var amenitiesProp))
            {
                var amenitiesList = JsonSerializer.Deserialize<List<string>>(amenitiesProp.GetRawText());
                existing.Amenities = (amenitiesList != null)
                    ? string.Join(",", amenitiesList.Select(a => a.Trim()))
                    : existing.Amenities;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/v1/roomtypes/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var existing = await _context.Rooms.FindAsync(id);
            if (existing == null) return NotFound();
            _context.Rooms.Remove(existing);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoom(int id)
        {
            var room = await _context.Rooms
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (room == null) return NotFound();

            var galleries = await _context.Galleries.Where(g => g.RoomId == id).ToListAsync();

            var roomDto = new RoomListItemDto
            {
                Id = room.Id,
                RoomType = room.RoomType?.Name ?? string.Empty,
                Description = room.RoomType?.Description ?? string.Empty,
                RoomNumber = room.RoomNumber,
                PricePerNight = room.PricePerNight,
                Capacity = room.Capacity,
                BedType = room.BedType ?? string.Empty,
                Size = room.Size ?? string.Empty,
                Floor = room.Floor,
                Status = room.Status ?? string.Empty,
                Amenities = string.IsNullOrEmpty(room.Amenities)
                    ? new List<string>()
                    : room.Amenities.Split(',').Select(a => a.Trim()).ToList(),
                Gallery = galleries.Select(g => new GalleryDto
                {
                    Id = g.Id,
                    Title = g.Title ?? string.Empty,
                    Img = g.Img ?? string.Empty,
                    Alt = g.Alt ?? string.Empty
                }).ToList()
            };
            return Ok(roomDto);
        }
    }
}
