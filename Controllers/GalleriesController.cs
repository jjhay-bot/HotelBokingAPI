using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Controllers
{
    [ApiController]
    [Route("api/v1/galleries")]
    public class GalleriesController : ControllerBase
    {
        private readonly ApiContext _context;
        public GalleriesController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var galleries = await _context.Galleries.ToListAsync();
            return Ok(galleries);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var gallery = await _context.Galleries.FindAsync(id);
            if (gallery == null) return NotFound();
            return Ok(gallery);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] Gallery gallery)
        {
            _context.Galleries.Add(gallery);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = gallery.Id }, gallery);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] Gallery gallery)
        {
            if (id != gallery.Id) return BadRequest();
            _context.Entry(gallery).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var gallery = await _context.Galleries.FindAsync(id);
            if (gallery == null) return NotFound();
            _context.Galleries.Remove(gallery);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
