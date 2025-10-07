using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Api.Data;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/roomtypes")]
    public class RoomTypesController : ControllerBase
    {
        private readonly ApiContext _context;
        public RoomTypesController(ApiContext context)
        {
            _context = context;
        }

        // GET: api/v1/roomtypes
        [HttpGet]
        public async Task<IActionResult> GetRoomTypes()
        {
            var roomTypes = await _context.RoomTypes.ToListAsync();
            return Ok(roomTypes);
        }

        // POST: api/v1/roomtypes
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRoomType([FromBody] RoomType roomType)
        {
            _context.RoomTypes.Add(roomType);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRoomTypes), new { id = roomType.Id }, roomType);
        }

        // PUT: api/v1/roomtypes/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRoomType(int id, [FromBody] RoomType roomType)
        {
            var existing = await _context.RoomTypes.FindAsync(id);
            if (existing == null) return NotFound();
            existing.Name = roomType.Name;
            existing.Description = roomType.Description;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PATCH: api/v1/roomtypes/{id}
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PatchRoomType(int id, [FromBody] JsonElement patchDoc)
        {
            var existing = await _context.RoomTypes.FindAsync(id);
            if (existing == null) return NotFound();

            // Only update provided fields
            if (patchDoc.TryGetProperty("name", out var nameProp))
                existing.Name = nameProp.GetString() ?? existing.Name;
            if (patchDoc.TryGetProperty("description", out var descProp))
                existing.Description = descProp.GetString() ?? existing.Description;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/v1/roomtypes/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRoomType(int id)
        {
            var existing = await _context.RoomTypes.FindAsync(id);
            if (existing == null) return NotFound();
            _context.RoomTypes.Remove(existing);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
