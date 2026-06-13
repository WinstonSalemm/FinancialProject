using FinancialProject.Domain.Common;
using FinancialProject.Domain.Contexts.Sales.Enums;

namespace FinancialProject.Domain.Contexts.Sales.Entities;

public class Deal : TenantSoftDeleteEntity<Guid>, IFinancialAggregateRoot
{
    private readonly List<DealItem> _items = [];
    private readonly List<DealCostLine> _costLines = [];
    private readonly List<DealObligation> _obligations = [];

    public Guid SourceLeadId { get; private set; }
    public Guid ClientId { get; private set; }
    public Guid OwnerMembershipId { get; private set; }
    public DealStatus Status { get; private set; }
    public string Title { get; private set; } = null!;
    public string? ContractNumber { get; private set; }
    public DateOnly PlannedExecutionDate { get; private set; }
    public decimal ManualCurrencyRate { get; private set; }
    public decimal CentralBankCurrencyRate { get; private set; }
    public decimal AppliedCurrencyRate { get; private set; }
    public decimal RiskPercent { get; private set; }
    public decimal MarkupPercent { get; private set; }
    public decimal VatRate { get; private set; }
    public decimal ProfitTaxRate { get; private set; }
    public IReadOnlyCollection<DealItem> Items => _items.AsReadOnly();
    public IReadOnlyCollection<DealCostLine> CostLines => _costLines.AsReadOnly();
    public IReadOnlyCollection<DealObligation> Obligations => _obligations.AsReadOnly();

    private Deal()
    {
    }

    public static Deal Create(
        Guid organizationId,
        Guid sourceLeadId,
        Guid clientId,
        Guid ownerMembershipId,
        string title,
        string? contractNumber,
        DateOnly plannedExecutionDate,
        decimal manualCurrencyRate,
        decimal centralBankCurrencyRate,
        decimal appliedCurrencyRate,
        decimal riskPercent,
        decimal markupPercent,
        decimal vatRate,
        decimal profitTaxRate,
        Guid createdBy)
    {
        var entity = new Deal
        {
            SourceLeadId = sourceLeadId,
            ClientId = clientId,
            OwnerMembershipId = ownerMembershipId,
            Status = DealStatus.PendingExecution,
            Title = string.IsNullOrWhiteSpace(title) ? throw new ArgumentException("Deal title is required.", nameof(title)) : title.Trim(),
            ContractNumber = string.IsNullOrWhiteSpace(contractNumber) ? null : contractNumber.Trim(),
            PlannedExecutionDate = plannedExecutionDate,
            ManualCurrencyRate = manualCurrencyRate,
            CentralBankCurrencyRate = centralBankCurrencyRate,
            AppliedCurrencyRate = appliedCurrencyRate,
            RiskPercent = riskPercent,
            MarkupPercent = markupPercent,
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
        Status = DealStatus.InProgress;
        MarkUpdated(updatedBy, DateTimeOffset.UtcNow);
    }

    public void Complete(Guid updatedBy)
    {
        Status = DealStatus.Completed;
        MarkUpdated(updatedBy, DateTimeOffset.UtcNow);
    }

    public void Fail(Guid updatedBy)
    {
        Status = DealStatus.Failed;
        MarkUpdated(updatedBy, DateTimeOffset.UtcNow);
    }
}

public class DealItem : TenantEntity<Guid>
{
    public Guid DealId { get; private set; }
    public Deal Deal { get; private set; } = null!;
    public string? PartNumber { get; private set; }
    public string Description { get; private set; } = null!;
    public decimal Quantity { get; private set; }
    public decimal PurchaseUnitPrice { get; private set; }
    public decimal? SalesUnitPrice { get; private set; }

    private DealItem()
    {
    }

    public static DealItem Create(
        Guid organizationId,
        Guid dealId,
        string? partNumber,
        string description,
        decimal quantity,
        decimal purchaseUnitPrice,
        decimal? salesUnitPrice,
        Guid createdBy)
    {
        var entity = new DealItem
        {
            DealId = dealId,
            PartNumber = string.IsNullOrWhiteSpace(partNumber) ? null : partNumber.Trim(),
            Description = string.IsNullOrWhiteSpace(description) ? throw new ArgumentException("Deal item description is required.", nameof(description)) : description.Trim(),
            Quantity = quantity,
            PurchaseUnitPrice = purchaseUnitPrice,
            SalesUnitPrice = salesUnitPrice
        };

        entity.SetIdentity(Guid.NewGuid());
        entity.SetOrganization(organizationId);
        entity.MarkCreated(createdBy, DateTimeOffset.UtcNow);
        return entity;
    }
}

public class DealCostLine : TenantEntity<Guid>
{
    public Guid DealId { get; private set; }
    public Deal Deal { get; private set; } = null!;
    public CostLineCategory Category { get; private set; }
    public string Description { get; private set; } = null!;
    public string? PerformerName { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal UnitAmount { get; private set; }

    private DealCostLine()
    {
    }

    public static DealCostLine Create(
        Guid organizationId,
        Guid dealId,
        CostLineCategory category,
        string description,
        string? performerName,
        decimal quantity,
        decimal unitAmount,
        Guid createdBy)
    {
        var entity = new DealCostLine
        {
            DealId = dealId,
            Category = category,
            Description = string.IsNullOrWhiteSpace(description) ? throw new ArgumentException("Deal cost line description is required.", nameof(description)) : description.Trim(),
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

public class DealObligation : TenantEntity<Guid>
{
    public Guid DealId { get; private set; }
    public Deal Deal { get; private set; } = null!;
    public string Title { get; private set; } = null!;
    public string? Description { get; private set; }
    public ObligationStatus Status { get; private set; }
    public DateOnly DueDate { get; private set; }

    private DealObligation()
    {
    }

    public static DealObligation Create(
        Guid organizationId,
        Guid dealId,
        string title,
        string? description,
        ObligationStatus status,
        DateOnly dueDate,
        Guid createdBy)
    {
        var entity = new DealObligation
        {
            DealId = dealId,
            Title = string.IsNullOrWhiteSpace(title) ? throw new ArgumentException("Obligation title is required.", nameof(title)) : title.Trim(),
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            Status = status,
            DueDate = dueDate
        };

        entity.SetIdentity(Guid.NewGuid());
        entity.SetOrganization(organizationId);
        entity.MarkCreated(createdBy, DateTimeOffset.UtcNow);
        return entity;
    }
}
