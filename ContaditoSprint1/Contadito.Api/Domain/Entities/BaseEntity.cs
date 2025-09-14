
using System;

namespace Contadito.Api.Domain.Entities
{
    public abstract class BaseEntity
    {
        public long Id { get; set; }
        public long TenantId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
    }
}
