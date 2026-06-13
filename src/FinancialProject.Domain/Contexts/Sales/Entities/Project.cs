using FinancialProject.Domain.Common;
using FinancialProject.Domain.Contexts.Sales.Enums;

namespace FinancialProject.Domain.Contexts.Sales.Entities;

public class Project : TenantSoftDeleteEntity<Guid>, IFinancialAggregateRoot
{
    private readonly List<ProjectCostLine> _costLines = [];

    public Guid SourceLeadId { get; private set; }
    public Guid ClientId { get; private set; }
    public Guid OwnerMembershipId { get; private set; }
    public ProjectStatus Status { get; private set; }
    public ProjectType Type { get; private set; }
    public string Title { get; private set; } = null!;
    public string? ContractNumber { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public decimal VatRate { get; private set; }
    public decimal ProfitTaxRate { get; private set; }
    public IReadOnlyCollection<ProjectCostLine> CostLines => _costLines.AsReadOnly();

    private Project()
    {
    }

    public static Project Create(
        Guid organizationId,
        Guid sourceLeadId,
        Guid clientId,
        Guid ownerMembershipId,
        ProjectType type,
        string title,
        string? contractNumber,
        DateOnly startDate,
        DateOnly endDate,
        decimal vatRate,
        decimal profitTaxRate,
        Guid createdBy)
    {
        var entity = new Project
        {
            SourceLeadId = sourceLeadId,
            ClientId = clientId,
            OwnerMembershipId = ownerMembershipId,
            Status = ProjectStatus.PendingExecution,
            Type = type,
            Title = string.IsNullOrWhiteSpace(title) ? throw new ArgumentException("Project title is required.", nameof(title)) : title.Trim(),
            ContractNumber = string.IsNullOrWhiteSpace(contractNumber) ? null : contractNumber.Trim(),
            StartDate = startDate,
            EndDate = endDate,
            VatRate = vatRate,
            ProfitTaxRate = profitTaxRate
        };

        entity.SetIdentity(Guid.NewGuid());
        entity.SetOrganization(organizationId);
        entity.MarkCreated(createdBy, DateTimeOffset.UtcNow);
        return entity;
    }

    public void Start(Guid updatedBy)
    {
        Status = ProjectStatus.InProgress;
        MarkUpdated(updatedBy, DateTimeOffset.UtcNow);
    }

    public void Complete(Guid updatedBy)
    {
        Status = ProjectStatus.Completed;
        MarkUpdated(updatedBy, DateTimeOffset.UtcNow);
    }

    public void Cancel(Guid updatedBy)
    {
        Status = ProjectStatus.Cancelled;
        MarkUpdated(updatedBy, DateTimeOffset.UtcNow);
    }
}

public class ProjectCostLine : TenantEntity<Guid>
{
    public Guid ProjectId { get; private set; }
    public Project Project { get; private set; } = null!;
    public CostLineCategory Category { get; private set; }
    public string Description { get; private set; } = null!;
    public string? PerformerName { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal UnitAmount { get; private set; }

    private ProjectCostLine()
    {
    }

    public static ProjectCostLine Create(
        Guid organizationId,
        Guid projectId,
        CostLineCategory category,
        string description,
        string? performerName,
        decimal quantity,
        decimal unitAmount,
        Guid createdBy)
    {
        var entity = new ProjectCostLine
        {
            ProjectId = projectId,
            Category = category,
            Description = string.IsNullOrWhiteSpace(description) ? throw new ArgumentException("Project cost line description is required.", nameof(description)) : description.Trim(),
            PerformerName = string.IsNullOrWhiteSpace(performerName) ? null : performerName.Trim(),
            Quantity = quantity,
            UnitAmount = unitAmount
        };

        entity.SetIdentity(Guid.NewGuid());
        entity.SetOrganization(organizationId);
        entity.MarkCreated(createdBy, DateTimeOffset.UtcNow);
        return entity;
    }
}
