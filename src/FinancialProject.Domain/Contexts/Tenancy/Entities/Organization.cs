using FinancialProject.Domain.Common;
using FinancialProject.Domain.Contexts.Tenancy.Enums;

namespace FinancialProject.Domain.Contexts.Tenancy.Entities;

public class Organization : SoftDeleteEntity<Guid>, IFinancialAggregateRoot
{
    private readonly List<OrganizationSettings> _settingsHistory = [];

    public string Name { get; private set; } = null!;
    public string Tin { get; private set; } = null!;
    public string Phone { get; private set; } = null!;
    public OrganizationType Type { get; private set; }
    public decimal CurrentBalance { get; private set; }
    public Guid? HoldingId { get; private set; }
    public Holding? Holding { get; private set; }
    public OrganizationSettings Settings { get; private set; } = null!;
    public IReadOnlyCollection<OrganizationSettings> SettingsHistory => _settingsHistory.AsReadOnly();

    private Organization()
    {
    }

    public static Organization Create(
        string name,
        string tin,
        string phone,
        OrganizationType type,
        Guid? holdingId,
        Guid createdBy,
        OrganizationSettings settings)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Organization name is required.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(tin))
        {
            throw new ArgumentException("Organization TIN is required.", nameof(tin));
        }

        if (string.IsNullOrWhiteSpace(phone))
        {
            throw new ArgumentException("Organization phone is required.", nameof(phone));
        }

        var entity = new Organization
        {
            Name = name.Trim(),
            Tin = tin.Trim(),
            Phone = phone.Trim(),
            Type = type,
            HoldingId = holdingId,
            CurrentBalance = 0m,
            Settings = settings
        };

        entity.SetIdentity(Guid.NewGuid());
        entity.MarkCreated(createdBy, DateTimeOffset.UtcNow);
        settings.AttachToOrganization(entity.Id, createdBy);
        return entity;
    }

    public void UpdateProfile(
        string name,
        string tin,
        string phone,
        OrganizationType type,
        Guid? holdingId,
        Guid updatedBy)
    {
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Organization name is required.", nameof(name)) : name.Trim();
        Tin = string.IsNullOrWhiteSpace(tin) ? throw new ArgumentException("Organization TIN is required.", nameof(tin)) : tin.Trim();
        Phone = string.IsNullOrWhiteSpace(phone) ? throw new ArgumentException("Organization phone is required.", nameof(phone)) : phone.Trim();
        Type = type;
        HoldingId = holdingId;
        MarkUpdated(updatedBy, DateTimeOffset.UtcNow);
    }

    public void UpdateBalance(decimal amountDelta, Guid updatedBy)
    {
        CurrentBalance += amountDelta;
        MarkUpdated(updatedBy, DateTimeOffset.UtcNow);
    }

    public void ReplaceSettings(OrganizationSettings settings, Guid updatedBy)
    {
        Settings = settings;
        settings.AttachToOrganization(Id, updatedBy);
        _settingsHistory.Add(settings);
        MarkUpdated(updatedBy, DateTimeOffset.UtcNow);
    }
}
