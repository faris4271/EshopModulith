using Microsoft.AspNetCore.Identity;
using Shared.DDD;

namespace IdentityModule.Domain
{
    public class AppUser : IdentityUser, IAggregate
    {
        private readonly List<IDomainEvent> _domainEvent;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Uri? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public string? ObjectId { get; set; }

        /// <summary>Timestamp when the user last changed their password</summary>
        public DateTime LastPasswordChangeDate { get; set; } = DateTime.UtcNow;

        // Navigation property for password history
        public virtual ICollection<PasswordHistory> PasswordHistories { get; set; } = new List<PasswordHistory>();

        public IReadOnlyList<IDomainEvent> Events => _domainEvent.AsReadOnly();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; }
        public string LasteModifiedBy { get; set; }
        public DateTime LasteModified { get; set; } = DateTime.UtcNow;

        public IDomainEvent[] ClearDomainEvent()
        {
            var DomainEvent = _domainEvent.ToArray();
            _domainEvent.Clear();
            return DomainEvent;
        }

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvent.Add(domainEvent);
        }


    }
}
