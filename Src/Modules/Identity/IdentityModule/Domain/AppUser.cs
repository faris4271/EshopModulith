using IdentityModule.Domain.Events;
using Microsoft.AspNetCore.Identity;
using Shared.DDD;
using System.ComponentModel.DataAnnotations;

namespace IdentityModule.Domain
{
    public class AppUser : IdentityUser, IHasDomainEvents, IAuditableEntity
    {
        private readonly List<IDomainEvent> _domainEvent;
        public Name? FirstName { get; set; }
        public Name? LastName { get; set; }
        public Uri? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }


        public Guid? VendorId { get; set; }
        public IList<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();

        public UserAddress DefaultShippingAddress { get; set; }

        public Guid? DefaultShippingAddressId { get; set; }

        public UserAddress DefaultBillingAddress { get; set; }

        public Guid? DefaultBillingAddressId { get; set; }
        public string? ObjectId { get; set; }

        /// <summary>Timestamp when the user last changed their password</summary>
        public DateTime LastPasswordChangeDate { get; set; } = DateTime.UtcNow;

        // Navigation property for password history
        public virtual ICollection<PasswordHistory> PasswordHistories { get; set; } = new List<PasswordHistory>();

        public virtual ICollection<UserRoles> Roles { get; set; } = new List<UserRoles>();

        public virtual ICollection<UserGroup> GroupUsers { get; set; } = new List<UserGroup>();

        public IReadOnlyList<IDomainEvent> Events => _domainEvent.AsReadOnly();

        [StringLength(450)]
        public string? Culture { get; set; } = "en-us";

        public IReadOnlyCollection<IDomainEvent> DomainEvents => throw new NotImplementedException();

        public DateTimeOffset CreatedOn { get; set; }

        public string? CreatedById { get; set; }

        public DateTimeOffset? LatestUpdatedOn { get; set; }

        public string? LatestUpdatedById { get; set; }

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

        public void RecordRegistered(string? tenantId = null)
        {
            AddDomainEvent(UserRegisteredEvent.Create(
                userId: Id,
                email: Email ?? string.Empty,
                firstName: FirstName.name,
                lastName: LastName.name,
                tenantId: tenantId));
        }

        /// <summary>Records PasswordChangedEvent. Call after password change.</summary>
        public void RecordPasswordChanged(bool wasReset = false)
        {
            AddDomainEvent(PasswordChangedEvent.Create(
                userId: Id,
                wasReset: wasReset
               ));
        }

        /// <summary>Sets user to active and records UserActivatedEvent.</summary>
        public void Activate(string? activatedBy = null, string? tenantId = null)
        {
            if (IsActive) return;
            IsActive = true;
            AddDomainEvent(UserActivatedEvent.Create(
                userId: Id,
                activatedBy: activatedBy,
                tenantId: tenantId));
        }

        /// <summary>Sets user to inactive and records UserDeactivatedEvent.</summary>
        public void Deactivate(string? deactivatedBy = null, string? reason = null, string? tenantId = null)
        {
            if (!IsActive) return;
            IsActive = false;
            AddDomainEvent(UserDeactivatedEvent.Create(
                userId: Id,
                deactivatedBy: deactivatedBy,
                reason: reason,
                tenantId: tenantId));
        }

        /// <summary>Records UserRoleAssignedEvent. Call after roles are assigned.</summary>
        public void RecordRolesAssigned(IEnumerable<string> assignedRoles, string? tenantId = null)
        {
            var rolesList = assignedRoles.ToList();
            if (rolesList.Count == 0) return;
            AddDomainEvent(UserRoleAssignedEvent.Create(
                userId: Id,
                assignedRoles: rolesList,
                tenantId: tenantId));
        }

        public void ClearDomainEvents()
        {
            _domainEvent.Clear();
        }
    }
}
