using Contadito.Api.Data;
using Contadito.Api.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Contadito.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("customers")]
    public class CustomersController : ControllerBase
    {
        private readonly AppDbContext _db;
        public CustomersController(AppDbContext db) => _db = db;

        private long TenantId => (long)(HttpContext.Items["TenantId"] ?? 0);

        [HttpGet]
        public async Task<ActionResult<object>> List([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? q = null)
        {
            var query = _db.Customers.AsNoTracking()
                .Where(c => c.TenantId == TenantId && c.DeletedAt == null);

            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(c => c.Name.Contains(q) || (c.Email != null && c.Email.Contains(q)));

            var total = await query.CountAsync();
            var items = await query.OrderByDescending(c => c.Id)
                                   .Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();
            return Ok(new { total, page, pageSize, items });
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> Create([FromBody] Customer c)
        {
            c.Id = 0;
            c.TenantId = TenantId;
            c.CreatedAt = DateTime.UtcNow;
            c.UpdatedAt = DateTime.UtcNow;
            _db.Customers.Add(c);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = c.Id }, c);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<Customer>> GetById([FromRoute] long id)
        {
            var c = await _db.Customers.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == TenantId && x.DeletedAt == null);
            if (c == null) return NotFound();
            return Ok(c);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult> Update([FromRoute] long id, [FromBody] Customer dto)
        {
            var c = await _db.Customers
                .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == TenantId && x.DeletedAt == null);
            if (c == null) return NotFound();

            c.Name = dto.Name;
            c.Email = dto.Email;
            c.Phone = dto.Phone;
            c.DocumentId = dto.DocumentId;
            c.Address = dto.Address;
            c.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult> SoftDelete([FromRoute] long id)
        {
            var c = await _db.Customers
                .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == TenantId && x.DeletedAt == null);
            if (c == null) return NotFound();
            c.DeletedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
