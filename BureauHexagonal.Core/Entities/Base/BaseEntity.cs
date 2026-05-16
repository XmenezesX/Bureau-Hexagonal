using BureauHexagonal.Core.Common.NotificationError;

namespace BureauHexagonal.Core.Entities.Base
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected private set; }
        public DateTimeOffset CreatedAt { get; protected private set; }
        public DateTimeOffset? UpdatedAt { get; protected private set; }
        public DateTimeOffset? DeletedAt { get; protected private set; }

        public abstract NotificationErrors Validate();
    }
}
