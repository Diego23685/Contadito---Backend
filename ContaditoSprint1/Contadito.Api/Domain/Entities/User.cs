
namespace Contadito.Api.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "viewer";
        public string Status { get; set; } = "active";
        public DateTime? LastLoginAt { get; set; }
    }
}
