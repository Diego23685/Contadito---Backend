// Controllers/DashboardController.cs
using Contadito.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Contadito.Api.Controllers;

[ApiController]
[Authorize]
[Route("dashboard")]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _db;
    public DashboardController(AppDbContext db) => _db = db;
    private long TenantId => (long)(HttpContext.Items["TenantId"] ?? 0);

    [HttpGet("overview")]
    public async Task<ActionResult<object>> GetOverview()
    {
        // Contadores base
        var totalProducts = await _db.Products.CountAsync(p => (long)p.TenantId == TenantId && p.DeletedAt == null);
        var totalCustomers = await _db.Customers.CountAsync(c => (long)c.TenantId == TenantId && c.DeletedAt == null);
        var totalWarehouses = await _db.Warehouses.CountAsync(w => (long)w.TenantId == TenantId && w.DeletedAt == null);
        var servicesCount = await _db.Products.CountAsync(p => (long)p.TenantId == TenantId && p.DeletedAt == null && p.IsService);
        var servicesShare = totalProducts == 0 ? 0 : Math.Round((double)servicesCount / totalProducts * 100, 1);

        // Listas recientes
        var recentProducts = await _db.Products
            .Where(p => (long)p.TenantId == TenantId && p.DeletedAt == null)
            .OrderByDescending(p => p.UpdatedAt ?? p.CreatedAt)
            .Select(p => new { p.Id, p.Sku, p.Name, p.IsService, p.Unit, p.CreatedAt, p.UpdatedAt })
            .Take(5)
            .ToListAsync();

        var recentCustomers = await _db.Customers
            .Where(c => (long)c.TenantId == TenantId && c.DeletedAt == null)
            .OrderByDescending(c => c.UpdatedAt ?? c.CreatedAt)
            .Select(c => new { c.Id, c.Name, c.Email, c.Phone, c.CreatedAt, c.UpdatedAt })
            .Take(5)
            .ToListAsync();

        // Placeholders de ventas/stock (se llenarán cuando agregues esas tablas)
        var salesToday = 0m;
        var salesMTD = 0m;
        var avgTicket = 0m;
        var lowStockCount = 0;

        return Ok(new
        {
            totals = new
            {
                totalProducts,
                totalCustomers,
                totalWarehouses,
                servicesShare
            },
            sales = new
            {
                salesToday,
                salesMTD,
                avgTicket
            },
            inventory = new
            {
                lowStockCount
            },
            recent = new
            {
                products = recentProducts,
                customers = recentCustomers
            }
        });
    }
}
