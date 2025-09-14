using Contadito.Api.Data;
using Contadito.Api.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Contadito.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("warehouses")]
    public class WarehousesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public WarehousesController(AppDbContext db) => _db = db;

        private long TenantId => (long)(HttpContext.Items["TenantId"] ?? 0);

        [HttpGet]
        public async Task<ActionResult<object>> List()
        {
            var items = await _db.Warehouses.AsNoTracking()
                .Where(w => w.TenantId == TenantId && w.DeletedAt == null)
                .ToListAsync();
            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<Warehouse>> Create([FromBody] Warehouse w)
        {
            w.Id = 0;
            w.TenantId = TenantId;
            w.CreatedAt = DateTime.UtcNow;
            w.UpdatedAt = DateTime.UtcNow;
            _db.Warehouses.Add(w);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = w.Id }, w);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<Warehouse>> GetById([FromRoute] long id)
        {
            var w = await _db.Warehouses.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == TenantId && x.DeletedAt == null);
            if (w == null) return NotFound();
            return Ok(w);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult> Update([FromRoute] long id, [FromBody] Warehouse dto)
        {
            var w = await _db.Warehouses
                .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == TenantId && x.DeletedAt == null);
            if (w == null) return NotFound();

            w.Name = dto.Name;
            w.Address = dto.Address;
            w.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult> SoftDelete([FromRoute] long id)
        {
            var w = await _db.Warehouses
                .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == TenantId && x.DeletedAt == null);
            if (w == null) return NotFound();
            w.DeletedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
