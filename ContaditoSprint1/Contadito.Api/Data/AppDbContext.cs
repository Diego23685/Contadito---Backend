// Data/AppDbContext.cs
using Contadito.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Contadito.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Tenant> Tenants => Set<Tenant>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Warehouse> Warehouses => Set<Warehouse>();
        // ... el resto de DbSets que ya tengas

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------- tenants ----------
            modelBuilder.Entity<Tenant>(e =>
            {
                e.ToTable("tenants");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.Name).HasColumnName("name");
                e.Property(x => x.LegalName).HasColumnName("legal_name");
                e.Property(x => x.TaxId).HasColumnName("tax_id");
                e.Property(x => x.Country).HasColumnName("country");
                e.Property(x => x.Phone).HasColumnName("phone");
                e.Property(x => x.Email).HasColumnName("email");
                e.Property(x => x.Plan).HasColumnName("plan");
                e.Property(x => x.Status).HasColumnName("status");
                e.Property(x => x.CreatedAt).HasColumnName("created_at");
                e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
                e.Property(x => x.DeletedAt).HasColumnName("deleted_at");
                e.HasIndex(x => x.Status).HasDatabaseName("idx_tenants_status");
                e.HasIndex(x => x.Name).IsUnique().HasDatabaseName("uq_tenants_name");
            });

            // ---------- users ----------
            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("users");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.TenantId).HasColumnName("tenant_id");
                e.Property(x => x.Name).HasColumnName("name");
                e.Property(x => x.Email).HasColumnName("email");
                e.Property(x => x.PasswordHash).HasColumnName("password_hash");
                e.Property(x => x.Role).HasColumnName("role");
                e.Property(x => x.Status).HasColumnName("status");
                e.Property(x => x.LastLoginAt).HasColumnName("last_login_at");
                e.Property(x => x.CreatedAt).HasColumnName("created_at");
                e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
                e.Property(x => x.DeletedAt).HasColumnName("deleted_at");
                e.HasIndex(x => new { x.TenantId, x.Email }).IsUnique().HasDatabaseName("uq_users_tenant_email");
                e.HasIndex(x => new { x.TenantId, x.Role }).HasDatabaseName("idx_users_tenant_role");
            });

            // ---------- products (resumen) ----------
            modelBuilder.Entity<Product>(e =>
            {
                e.ToTable("products");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.TenantId).HasColumnName("tenant_id");
                e.Property(x => x.Sku).HasColumnName("sku");
                e.Property(x => x.Name).HasColumnName("name");
                e.Property(x => x.CategoryId).HasColumnName("category_id");
                e.Property(x => x.Description).HasColumnName("description");
                e.Property(x => x.Unit).HasColumnName("unit");
                e.Property(x => x.Barcode).HasColumnName("barcode");
                e.Property(x => x.IsService).HasColumnName("is_service");
                e.Property(x => x.TrackStock).HasColumnName("track_stock");
                e.Property(x => x.CreatedAt).HasColumnName("created_at");
                e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
                e.Property(x => x.DeletedAt).HasColumnName("deleted_at");
            });

            // ---------- customers (resumen) ----------
            modelBuilder.Entity<Customer>(e =>
            {
                e.ToTable("customers");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.TenantId).HasColumnName("tenant_id");
                e.Property(x => x.Name).HasColumnName("name");
                e.Property(x => x.Email).HasColumnName("email");
                e.Property(x => x.Phone).HasColumnName("phone");
                e.Property(x => x.DocumentId).HasColumnName("document_id");
                e.Property(x => x.Address).HasColumnName("address");
                e.Property(x => x.CreatedAt).HasColumnName("created_at");
                e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
                e.Property(x => x.DeletedAt).HasColumnName("deleted_at");
            });

            // ---------- warehouses (resumen) ----------
            modelBuilder.Entity<Warehouse>(e =>
            {
                e.ToTable("warehouses");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.TenantId).HasColumnName("tenant_id");
                e.Property(x => x.Name).HasColumnName("name");
                e.Property(x => x.Address).HasColumnName("address");
                e.Property(x => x.CreatedAt).HasColumnName("created_at");
                e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
                e.Property(x => x.DeletedAt).HasColumnName("deleted_at");
            });

            // (Puedes ir agregando el resto con el mismo patrón)
        }
    }
}
