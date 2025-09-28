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
        public async Task<IActionResult> GetRooms()
        {
            var rooms = await _context.Rooms.ToListAsync();
            var roomDtos = rooms.Select(r => new RoomDto
            {
                Id = r.Id,
                RoomTypeId = r.RoomTypeId,
                RoomNumber = r.RoomNumber,
                PricePerNight = r.PricePerNight,
                Capacity = r.Capacity,
                BedType = r.BedType,
                Size = r.Size,
                Floor = r.Floor,
                Status = r.Status,
                Amenities = string.IsNullOrEmpty(r.Amenities)
                    ? new List<string>()
                    : r.Amenities.Split(',').Select(a => a.Trim()).ToList(),
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                OccupiedByUserId = r.OccupiedByUserId
            }).ToList();
            return Ok(roomDtos);
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
    }
}
