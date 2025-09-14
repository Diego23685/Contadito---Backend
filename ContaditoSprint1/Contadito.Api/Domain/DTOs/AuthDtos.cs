
namespace Contadito.Api.Domain.DTOs
{
    public record RegisterTenantRequest(string TenantName, string OwnerName, string OwnerEmail, string Password);
    public record LoginRequest(string Email, string Password);
    public record AuthResponse(string AccessToken, long ExpiresIn);
}
