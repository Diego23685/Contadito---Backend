
namespace Contadito.Api.Domain.DTOs
{
    public record ProductCreateDto(string Sku, string Name, string? Description, string Unit, bool IsService, bool TrackStock);
    public record ProductUpdateDto(string Name, string? Description, string Unit, bool IsService, bool TrackStock);
}
