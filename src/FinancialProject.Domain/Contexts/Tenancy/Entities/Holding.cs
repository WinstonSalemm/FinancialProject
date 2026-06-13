using FinancialProject.Domain.Common;

namespace FinancialProject.Domain.Contexts.Tenancy.Entities;

public class Holding : SoftDeleteEntity<Guid>, IFinancialAggregateRoot
{
    private readonly List<Organization> _organizations = [];

    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public IReadOnlyCollection<Organization> Organizations => _organizations.AsReadOnly();

    private Holding()
    {
    }

    public static Holding Create(string name, string? description, Guid createdBy)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Holding name is required.", nameof(name));
        }

        var entity = new Holding
        {
            Name = name.Trim(),
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim()
        };

        entity.SetIdentity(Guid.NewGuid());
        entity.MarkCreated(createdBy, DateTimeOffset.UtcNow);
        return entity;
    }

    public void Update(string name, string? description, Guid updatedBy)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Holding name is required.", nameof(name));
        }

        Name = name.Trim();
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        MarkUpdated(updatedBy, DateTimeOffset.UtcNow);
    }
}
