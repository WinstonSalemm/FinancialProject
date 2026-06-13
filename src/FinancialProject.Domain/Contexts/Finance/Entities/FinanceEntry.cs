using FinancialProject.Domain.Common;
using FinancialProject.Domain.Contexts.Finance.Enums;

namespace FinancialProject.Domain.Contexts.Finance.Entities;

public class FinanceEntry : TenantSoftDeleteEntity<Guid>, IFinancialAggregateRoot
{
    public Guid? LeadId { get; private set; }
    public Guid? DealId { get; private set; }
    public Guid? ProjectId { get; private set; }
    public Guid? AdminExpenseId { get; private set; }
    public Guid? FinancialAdjustmentId { get; private set; }
    public Guid? ClientId { get; private set; }
    public FinanceEntryType Type { get; private set; }
    public FinanceFlowDirection Direction { get; private set; }
    public string Description { get; private set; } = null!;
    public string CurrencyCode { get; private set; } = "UZS";
    public decimal ExchangeRate { get; private set; } = 1m;
    public decimal Amount { get; private set; }
    public decimal BaseCurrencyAmount { get; private set; }
    public DateOnly OccurredOn { get; private set; }
    public bool IsForecast { get; private set; }
    public string? ExternalReference { get; private set; }

    private FinanceEntry()
    {
    }

    public static FinanceEntry Create(
        Guid organizationId,
        FinanceEntryType type,
        FinanceFlowDirection direction,
        string description,
        string currencyCode,
        decimal exchangeRate,
        decimal amount,
        decimal baseCurrencyAmount,
        DateOnly occurredOn,
        bool isForecast,
        Guid createdBy,
        Guid? leadId = null,
        Guid? dealId = null,
        Guid? projectId = null,
        Guid? adminExpenseId = null,
        Guid? financialAdjustmentId = null,
        Guid? clientId = null,
        string? externalReference = null)
    {
        var entity = new FinanceEntry
        {
            LeadId = leadId,
            DealId = dealId,
            ProjectId = projectId,
            AdminExpenseId = adminExpenseId,
            FinancialAdjustmentId = financialAdjustmentId,
            ClientId = clientId,
            Type = type,
            Direction = direction,
            Description = string.IsNullOrWhiteSpace(description) ? throw new ArgumentException("Finance entry description is required.", nameof(description)) : description.Trim(),
            CurrencyCode = string.IsNullOrWhiteSpace(currencyCode) ? "UZS" : currencyCode.Trim().ToUpperInvariant(),
            ExchangeRate = exchangeRate <= 0 ? 1m : exchangeRate,
            Amount = amount,
            BaseCurrencyAmount = baseCurrencyAmount,
            OccurredOn = occurredOn,
            IsForecast = isForecast,
            ExternalReference = string.IsNullOrWhiteSpace(externalReference) ? null : externalReference.Trim()
        };

        entity.SetIdentity(Guid.NewGuid());
        entity.SetOrganization(organizationId);
        entity.MarkCreated(createdBy, DateTimeOffset.UtcNow);
        return entity;
    }
}

public class AdminExpenseCategory : TenantSoftDeleteEntity<Guid>
{
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsSystem { get; private set; }

    private AdminExpenseCategory()
    {
    }

    public static AdminExpenseCategory Create(Guid organizationId, string code, string name, string? description, bool isSystem, Guid createdBy)
    {
        var entity = new AdminExpenseCategory
        {
            Code = NormalizeCode(code),
            Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Admin expense category name is required.", nameof(name)) : name.Trim(),
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            IsSystem = isSystem
        };

        entity.SetIdentity(Guid.NewGuid());
        entity.SetOrganization(organizationId);
        entity.MarkCreated(createdBy, DateTimeOffset.UtcNow);
        return entity;
    }

    public void Update(string code, string name, string? description, Guid updatedBy)
    {
        Code = NormalizeCode(code);
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Admin expense category name is required.", nameof(name)) : name.Trim();
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        MarkUpdated(updatedBy, DateTimeOffset.UtcNow);
    }

    private static string NormalizeCode(string code)
    {
        return string.IsNullOrWhiteSpace(code)
            ? throw new ArgumentException("Admin expense category code is required.", nameof(code))
            : code.Trim().ToUpperInvariant();
    }
}

public class AdminExpense : TenantSoftDeleteEntity<Guid>, IFinancialAggregateRoot
{
    public Guid CategoryId { get; private set; }
    public AdminExpenseCategory Category { get; private set; } = null!;
    public string Title { get; private set; } = null!;
    public string? Notes { get; private set; }
    public decimal Amount { get; private set; }
    public DateOnly ExpenseDate { get; private set; }
    public int PeriodYear { get; private set; }
    public int PeriodMonth { get; private set; }
    public AdminExpenseStatus Status { get; private set; }

    private AdminExpense()
    {
    }

    public static AdminExpense Create(
        Guid organizationId,
        Guid categoryId,
        string title,
        string? notes,
        decimal amount,
        DateOnly expenseDate,
        int periodYear,
        int periodMonth,
        Guid createdBy)
    {
        var entity = new AdminExpense
        {
            CategoryId = categoryId,
            Title = string.IsNullOrWhiteSpace(title) ? throw new ArgumentException("Admin expense title is required.", nameof(title)) : title.Trim(),
            Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim(),
            Amount = amount,
            ExpenseDate = expenseDate,
            PeriodYear = periodYear,
            PeriodMonth = periodMonth,
            Status = AdminExpenseStatus.Draft
        };

        entity.SetIdentity(Guid.NewGuid());
        entity.SetOrganization(organizationId);
        entity.MarkCreated(createdBy, DateTimeOffset.UtcNow);
        return entity;
    }

    public void Post(Guid updatedBy)
    {
        Status = AdminExpenseStatus.Posted;
        MarkUpdated(updatedBy, DateTimeOffset.UtcNow);
    }
}

public class TaxRate : TenantSoftDeleteEntity<Guid>, IFinancialAggregateRoot
{
    public TaxRateType Type { get; private set; }
    public string Name { get; private set; } = null!;
    public decimal Rate { get; private set; }
    public DateOnly EffectiveFrom { get; private set; }
    public DateOnly? EffectiveTo { get; private set; }

    private TaxRate()
    {
    }

    public static TaxRate Create(
        Guid organizationId,
        TaxRateType type,
        string name,
        decimal rate,
        DateOnly effectiveFrom,
        DateOnly? effectiveTo,
        Guid createdBy)
    {
        var entity = new TaxRate
        {
            Type = type,
            Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Tax rate name is required.", nameof(name)) : name.Trim(),
            Rate = rate,
            EffectiveFrom = effectiveFrom,
            EffectiveTo = effectiveTo
        };

        entity.SetIdentity(Guid.NewGuid());
        entity.SetOrganization(organizationId);
        entity.MarkCreated(createdBy, DateTimeOffset.UtcNow);
        return entity;
    }
}

public class FinancialAdjustment : TenantSoftDeleteEntity<Guid>, IFinancialAggregateRoot
{
    public Guid? DealId { get; private set; }
    public Guid? ProjectId { get; private set; }
    public DateOnly AdjustmentDate { get; private set; }
    public string Reason { get; private set; } = null!;
    public decimal ImpactAmount { get; private set; }
    public string? Comment { get; private set; }

    private FinancialAdjustment()
    {
    }

    public static FinancialAdjustment Create(
        Guid organizationId,
        DateOnly adjustmentDate,
        string reason,
        decimal impactAmount,
        string? comment,
        Guid createdBy,
        Guid? dealId = null,
        Guid? projectId = null)
    {
        var entity = new FinancialAdjustment
        {
            DealId = dealId,
            ProjectId = projectId,
            AdjustmentDate = adjustmentDate,
            Reason = string.IsNullOrWhiteSpace(reason) ? throw new ArgumentException("Financial adjustment reason is required.", nameof(reason)) : reason.Trim(),
            ImpactAmount = impactAmount,
            Comment = string.IsNullOrWhiteSpace(comment) ? null : comment.Trim()
        };

        entity.SetIdentity(Guid.NewGuid());
        entity.SetOrganization(organizationId);
        entity.MarkCreated(createdBy, DateTimeOffset.UtcNow);
        return entity;
    }
}
