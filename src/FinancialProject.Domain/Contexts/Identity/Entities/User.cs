using FinancialProject.Domain.Common;

namespace FinancialProject.Domain.Contexts.Identity.Entities;

public class User : SoftDeleteEntity<Guid>, IFinancialAggregateRoot
{
    private readonly List<OrganizationMembership> _memberships = [];

    public string FullName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string? PhoneNumber { get; private set; }
    public string PasswordHash { get; private set; } = null!;
    public bool IsActive { get; private set; } = true;
    public DateTimeOffset? LastGlobalLoginAt { get; private set; }
    public IReadOnlyCollection<OrganizationMembership> Memberships => _memberships.AsReadOnly();

    private User()
    {
    }

    public static User Create(string fullName, string email, string passwordHash, string? phoneNumber, Guid createdBy)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException("Full name is required.", nameof(fullName));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email is required.", nameof(email));
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new ArgumentException("Password hash is required.", nameof(passwordHash));
        }

        var entity = new User
        {
            FullName = fullName.Trim(),
            Email = email.Trim().ToLowerInvariant(),
            PasswordHash = passwordHash,
            PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? null : phoneNumber.Trim()
        };

        entity.SetIdentity(Guid.NewGuid());
        entity.MarkCreated(createdBy, DateTimeOffset.UtcNow);
        return entity;
    }

    public void UpdateProfile(string fullName, string email, string? phoneNumber, Guid updatedBy)
    {
        FullName = string.IsNullOrWhiteSpace(fullName) ? throw new ArgumentException("Full name is required.", nameof(fullName)) : fullName.Trim();
        Email = string.IsNullOrWhiteSpace(email) ? throw new ArgumentException("Email is required.", nameof(email)) : email.Trim().ToLowerInvariant();
        PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? null : phoneNumber.Trim();
        MarkUpdated(updatedBy, DateTimeOffset.UtcNow);
    }

    public void UpdatePasswordHash(string passwordHash, Guid updatedBy)
    {
        PasswordHash = string.IsNullOrWhiteSpace(passwordHash) ? throw new ArgumentException("Password hash is required.", nameof(passwordHash)) : passwordHash;
        MarkUpdated(updatedBy, DateTimeOffset.UtcNow);
    }

    public void SetActivity(bool isActive, Guid updatedBy)
    {
        IsActive = isActive;
        MarkUpdated(updatedBy, DateTimeOffset.UtcNow);
    }

    public void RegisterLogin(Guid updatedBy)
    {
        LastGlobalLoginAt = DateTimeOffset.UtcNow;
        MarkUpdated(updatedBy, LastGlobalLoginAt.Value);
    }
}
