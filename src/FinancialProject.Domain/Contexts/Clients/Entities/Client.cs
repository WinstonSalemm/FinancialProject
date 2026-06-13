using FinancialProject.Domain.Common;

namespace FinancialProject.Domain.Contexts.Clients.Entities;

public class Client : TenantSoftDeleteEntity<Guid>, IFinancialAggregateRoot
{
    public string CompanyName { get; private set; } = null!;
    public string Tin { get; private set; } = null!;
    public string Phone { get; private set; } = null!;
    public string? Complexity { get; private set; }
    public string? Notes { get; private set; }
    public string? Features { get; private set; }

    private Client()
    {
    }

    public static Client Create(
        Guid organizationId,
        string companyName,
        string tin,
        string phone,
        string? complexity,
        string? notes,
        string? features,
        Guid createdBy)
    {
        if (string.IsNullOrWhiteSpace(companyName))
        {
            throw new ArgumentException("Client company name is required.", nameof(companyName));
        }

        if (string.IsNullOrWhiteSpace(tin))
        {
            throw new ArgumentException("Client TIN is required.", nameof(tin));
        }

        if (string.IsNullOrWhiteSpace(phone))
        {
            throw new ArgumentException("Client phone is required.", nameof(phone));
        }

        var entity = new Client
        {
            CompanyName = companyName.Trim(),
            Tin = tin.Trim(),
            Phone = phone.Trim(),
            Complexity = Normalize(complexity),
            Notes = Normalize(notes),
            Features = Normalize(features)
        };

        entity.SetIdentity(Guid.NewGuid());
        entity.SetOrganization(organizationId);
        entity.MarkCreated(createdBy, DateTimeOffset.UtcNow);
        return entity;
    }

    public void Update(
        string companyName,
        string tin,
        string phone,
        string? complexity,
        string? notes,
        string? features,
        Guid updatedBy)
    {
        CompanyName = string.IsNullOrWhiteSpace(companyName) ? throw new ArgumentException("Client company name is required.", nameof(companyName)) : companyName.Trim();
        Tin = string.IsNullOrWhiteSpace(tin) ? throw new ArgumentException("Client TIN is required.", nameof(tin)) : tin.Trim();
        Phone = string.IsNullOrWhiteSpace(phone) ? throw new ArgumentException("Client phone is required.", nameof(phone)) : phone.Trim();
        Complexity = Normalize(complexity);
        Notes = Normalize(notes);
        Features = Normalize(features);
        MarkUpdated(updatedBy, DateTimeOffset.UtcNow);
    }

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
