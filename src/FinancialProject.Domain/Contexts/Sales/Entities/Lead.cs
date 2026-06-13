using FinancialProject.Domain.Common;
using FinancialProject.Domain.Contexts.Sales.Enums;

namespace FinancialProject.Domain.Contexts.Sales.Entities;

public class Lead : TenantSoftDeleteEntity<Guid>, IFinancialAggregateRoot
{
    private readonly List<LeadApprovalRecord> _approvalHistory = [];
    private readonly List<PrivateNote> _privateNotes = [];

    public Guid ClientId { get; private set; }
    public Guid OwnerMembershipId { get; private set; }
    public LeadType Type { get; private set; }
    public LeadStatus Status { get; private set; }
    public string Title { get; private set; } = null!;
    public string? Description { get; private set; }
    public DateTimeOffset? ExpectedAt { get; private set; }
    public string? RejectionReason { get; private set; }
    public string? RejectionComment { get; private set; }
    public DealLeadDetails? DealDetails { get; private set; }
    public ProjectLeadDetails? ProjectDetails { get; private set; }
    public IReadOnlyCollection<LeadApprovalRecord> ApprovalHistory => _approvalHistory.AsReadOnly();
    public IReadOnlyCollection<PrivateNote> PrivateNotes => _privateNotes.AsReadOnly();

    private Lead()
    {
    }

    public static Lead Create(
        Guid organizationId,
        Guid clientId,
        Guid ownerMembershipId,
        LeadType type,
        string title,
        string? description,
        DateTimeOffset? expectedAt,
        Guid createdBy)
    {
        var entity = new Lead
        {
            ClientId = clientId,
            OwnerMembershipId = ownerMembershipId,
            Type = type,
            Status = LeadStatus.Draft,
            Title = string.IsNullOrWhiteSpace(title) ? throw new ArgumentException("Lead title is required.", nameof(title)) : title.Trim(),
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            ExpectedAt = expectedAt
        };

        entity.SetIdentity(Guid.NewGuid());
        entity.SetOrganization(organizationId);
        entity.MarkCreated(createdBy, DateTimeOffset.UtcNow);
        return entity;
    }

    public void UpdateBasics(string title, string? description, DateTimeOffset? expectedAt, Guid updatedBy)
    {
        Title = string.IsNullOrWhiteSpace(title) ? throw new ArgumentException("Lead title is required.", nameof(title)) : title.Trim();
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        ExpectedAt = expectedAt;
        MarkUpdated(updatedBy, DateTimeOffset.UtcNow);
    }

    public void AttachDealDetails(DealLeadDetails details, Guid updatedBy)
    {
        if (Type != LeadType.Deal)
        {
            throw new InvalidOperationException("Deal details can be attached only to deal leads.");
        }

        DealDetails = details;
        MarkUpdated(updatedBy, DateTimeOffset.UtcNow);
    }

    public void AttachProjectDetails(ProjectLeadDetails details, Guid updatedBy)
    {
        if (Type != LeadType.Project)
        {
            throw new InvalidOperationException("Project details can be attached only to project leads.");
        }

        ProjectDetails = details;
        MarkUpdated(updatedBy, DateTimeOffset.UtcNow);
    }

    public void MoveToFinanceReview(Guid updatedBy)
    {
        Status = LeadStatus.FinanceReview;
        MarkUpdated(updatedBy, DateTimeOffset.UtcNow);
    }

    public void MoveToDirectorReview(Guid updatedBy)
    {
        Status = LeadStatus.DirectorReview;
        MarkUpdated(updatedBy, DateTimeOffset.UtcNow);
    }

    public void Approve(Guid updatedBy)
    {
        Status = LeadStatus.Approved;
        RejectionReason = null;
        RejectionComment = null;
        MarkUpdated(updatedBy, DateTimeOffset.UtcNow);
    }

    public void Reject(string reason, string? comment, Guid updatedBy)
    {
        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new ArgumentException("Rejection reason is required.", nameof(reason));
        }

        Status = LeadStatus.Rejected;
        RejectionReason = reason.Trim();
        RejectionComment = string.IsNullOrWhiteSpace(comment) ? null : comment.Trim();
        MarkUpdated(updatedBy, DateTimeOffset.UtcNow);
    }
}

public class DealLeadDetails : TenantEntity<Guid>
{
    private readonly List<DealLeadItem> _items = [];
    private readonly List<LeadCostLine> _costLines = [];

    public Guid LeadId { get; private set; }
    public Lead Lead { get; private set; } = null!;
    public decimal ManualCurrencyRate { get; private set; }
    public decimal CentralBankCurrencyRate { get; private set; }
    public decimal AppliedCurrencyRate { get; private set; }
    public decimal RiskPercent { get; private set; }
    public decimal MarkupPercent { get; private set; }
    public decimal VatRate { get; private set; }
    public decimal ProfitTaxRate { get; private set; }
    public decimal BankExpenseRate { get; private set; }
    public DateOnly ExpectedDealDate { get; private set; }
    public IReadOnlyCollection<DealLeadItem> Items => _items.AsReadOnly();
    public IReadOnlyCollection<LeadCostLine> CostLines => _costLines.AsReadOnly();

    private DealLeadDetails()
    {
    }

    public static DealLeadDetails Create(
        Guid organizationId,
        Guid leadId,
        decimal manualCurrencyRate,
        decimal centralBankCurrencyRate,
        decimal appliedCurrencyRate,
        decimal riskPercent,
        decimal markupPercent,
        decimal vatRate,
        decimal profitTaxRate,
        decimal bankExpenseRate,
        DateOnly expectedDealDate,
        Guid createdBy)
    {
        var entity = new DealLeadDetails
        {
            LeadId = leadId,
            ManualCurrencyRate = manualCurrencyRate,
            CentralBankCurrencyRate = centralBankCurrencyRate,
            AppliedCurrencyRate = appliedCurrencyRate,
            RiskPercent = riskPercent,
            MarkupPercent = markupPercent,
            VatRate = vatRate,
            ProfitTaxRate = profitTaxRate,
            BankExpenseRate = bankExpenseRate,
            ExpectedDealDate = expectedDealDate
        };

        entity.SetIdentity(Guid.NewGuid());
        entity.SetOrganization(organizationId);
        entity.MarkCreated(createdBy, DateTimeOffset.UtcNow);
        return entity;
    }
}

public class DealLeadItem : TenantEntity<Guid>
{
    public Guid DealLeadDetailsId { get; private set; }
    public DealLeadDetails DealLeadDetails { get; private set; } = null!;
    public string? PartNumber { get; private set; }
    public string Description { get; private set; } = null!;
    public decimal Quantity { get; private set; }
    public decimal? ListPrice { get; private set; }
    public decimal? SupplierDiscountPercent { get; private set; }
    public decimal PurchaseUnitPrice { get; private set; }
    public decimal? RoyaltyPercent { get; private set; }

    private DealLeadItem()
    {
    }

    public static DealLeadItem Create(
        Guid organizationId,
        Guid dealLeadDetailsId,
        string? partNumber,
        string description,
        decimal quantity,
        decimal purchaseUnitPrice,
        decimal? listPrice,
        decimal? supplierDiscountPercent,
        decimal? royaltyPercent,
        Guid createdBy)
    {
        var entity = new DealLeadItem
        {
            DealLeadDetailsId = dealLeadDetailsId,
            PartNumber = Normalize(partNumber),
            Description = string.IsNullOrWhiteSpace(description) ? throw new ArgumentException("Deal lead item description is required.", nameof(description)) : description.Trim(),
            Quantity = quantity,
            PurchaseUnitPrice = purchaseUnitPrice,
            ListPrice = listPrice,
            SupplierDiscountPercent = supplierDiscountPercent,
            RoyaltyPercent = royaltyPercent
        };

        entity.SetIdentity(Guid.NewGuid());
        entity.SetOrganization(organizationId);
        entity.MarkCreated(createdBy, DateTimeOffset.UtcNow);
        return entity;
    }

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}

public class ProjectLeadDetails : TenantEntity<Guid>
{
    private readonly List<LeadCostLine> _costLines = [];

    public Guid LeadId { get; private set; }
    public Lead Lead { get; private set; } = null!;
    public ProjectType ProjectType { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public decimal VatRate { get; private set; }
    public decimal ProfitTaxRate { get; private set; }
    public IReadOnlyCollection<LeadCostLine> CostLines => _costLines.AsReadOnly();

    private ProjectLeadDetails()
    {
    }

    public static ProjectLeadDetails Create(
        Guid organizationId,
        Guid leadId,
        ProjectType projectType,
        DateOnly startDate,
        DateOnly endDate,
        decimal vatRate,
        decimal profitTaxRate,
        Guid createdBy)
    {
        var entity = new ProjectLeadDetails
        {
            LeadId = leadId,
            ProjectType = projectType,
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
}

public class LeadCostLine : TenantEntity<Guid>
{
    public Guid? DealLeadDetailsId { get; private set; }
    public DealLeadDetails? DealLeadDetails { get; private set; }
    public Guid? ProjectLeadDetailsId { get; private set; }
    public ProjectLeadDetails? ProjectLeadDetails { get; private set; }
    public CostLineCategory Category { get; private set; }
    public string Description { get; private set; } = null!;
    public string? PerformerName { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal UnitAmount { get; private set; }

    private LeadCostLine()
    {
    }

    public static LeadCostLine CreateForDealLead(
        Guid organizationId,
        Guid dealLeadDetailsId,
        CostLineCategory category,
        string description,
        string? performerName,
        decimal quantity,
        decimal unitAmount,
        Guid createdBy)
    {
        return CreateInternal(organizationId, dealLeadDetailsId, null, category, description, performerName, quantity, unitAmount, createdBy);
    }

    public static LeadCostLine CreateForProjectLead(
        Guid organizationId,
        Guid projectLeadDetailsId,
        CostLineCategory category,
        string description,
        string? performerName,
        decimal quantity,
        decimal unitAmount,
        Guid createdBy)
    {
        return CreateInternal(organizationId, null, projectLeadDetailsId, category, description, performerName, quantity, unitAmount, createdBy);
    }

    private static LeadCostLine CreateInternal(
        Guid organizationId,
        Guid? dealLeadDetailsId,
        Guid? projectLeadDetailsId,
        CostLineCategory category,
        string description,
        string? performerName,
        decimal quantity,
        decimal unitAmount,
        Guid createdBy)
    {
        var entity = new LeadCostLine
        {
            DealLeadDetailsId = dealLeadDetailsId,
            ProjectLeadDetailsId = projectLeadDetailsId,
            Category = category,
            Description = string.IsNullOrWhiteSpace(description) ? throw new ArgumentException("Lead cost line description is required.", nameof(description)) : description.Trim(),
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

public class LeadApprovalRecord : TenantEntity<Guid>
{
    public Guid LeadId { get; private set; }
    public Lead Lead { get; private set; } = null!;
    public ApprovalStage Stage { get; private set; }
    public ApprovalDecision Decision { get; private set; }
    public Guid ReviewerMembershipId { get; private set; }
    public string? Comment { get; private set; }
    public string? Reason { get; private set; }

    private LeadApprovalRecord()
    {
    }

    public static LeadApprovalRecord Create(
        Guid organizationId,
        Guid leadId,
        ApprovalStage stage,
        ApprovalDecision decision,
        Guid reviewerMembershipId,
        string? comment,
        string? reason,
        Guid createdBy)
    {
        var entity = new LeadApprovalRecord
        {
            LeadId = leadId,
            Stage = stage,
            Decision = decision,
            ReviewerMembershipId = reviewerMembershipId,
            Comment = string.IsNullOrWhiteSpace(comment) ? null : comment.Trim(),
            Reason = string.IsNullOrWhiteSpace(reason) ? null : reason.Trim()
        };

        entity.SetIdentity(Guid.NewGuid());
        entity.SetOrganization(organizationId);
        entity.MarkCreated(createdBy, DateTimeOffset.UtcNow);
        return entity;
    }
}

public class PrivateNote : TenantEntity<Guid>
{
    public Guid AuthorMembershipId { get; private set; }
    public Guid? LeadId { get; private set; }
    public Guid? DealId { get; private set; }
    public Guid? ProjectId { get; private set; }
    public string Body { get; private set; } = null!;

    private PrivateNote()
    {
    }

    public static PrivateNote CreateForLead(Guid organizationId, Guid authorMembershipId, Guid leadId, string body, Guid createdBy)
    {
        return CreateInternal(organizationId, authorMembershipId, leadId, null, null, body, createdBy);
    }

    public static PrivateNote CreateForDeal(Guid organizationId, Guid authorMembershipId, Guid dealId, string body, Guid createdBy)
    {
        return CreateInternal(organizationId, authorMembershipId, null, dealId, null, body, createdBy);
    }

    public static PrivateNote CreateForProject(Guid organizationId, Guid authorMembershipId, Guid projectId, string body, Guid createdBy)
    {
        return CreateInternal(organizationId, authorMembershipId, null, null, projectId, body, createdBy);
    }

    private static PrivateNote CreateInternal(
        Guid organizationId,
        Guid authorMembershipId,
        Guid? leadId,
        Guid? dealId,
        Guid? projectId,
        string body,
        Guid createdBy)
    {
        var entity = new PrivateNote
        {
            AuthorMembershipId = authorMembershipId,
            LeadId = leadId,
            DealId = dealId,
            ProjectId = projectId,
            Body = string.IsNullOrWhiteSpace(body) ? throw new ArgumentException("Private note body is required.", nameof(body)) : body.Trim()
        };

        entity.SetIdentity(Guid.NewGuid());
        entity.SetOrganization(organizationId);
        entity.MarkCreated(createdBy, DateTimeOffset.UtcNow);
        return entity;
    }
}
