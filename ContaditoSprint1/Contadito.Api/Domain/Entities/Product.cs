
namespace Contadito.Api.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Sku { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public long? CategoryId { get; set; }
        public string? Description { get; set; }
        public string Unit { get; set; } = "unidad";
        public string? Barcode { get; set; }
        public bool IsService { get; set; } = false;
        public bool TrackStock { get; set; } = true;
    }
}
