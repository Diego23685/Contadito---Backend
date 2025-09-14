
namespace Contadito.Api.Domain.Entities
{
    public class Tenant
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? LegalName { get; set; }
        public string? TaxId { get; set; }
        public string Country { get; set; } = "NI";
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string Plan { get; set; } = "free";
        public string Status { get; set; } = "active";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
    }
}
