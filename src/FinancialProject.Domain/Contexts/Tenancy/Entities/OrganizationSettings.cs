using FinancialProject.Domain.Common;
using FinancialProject.Domain.Contexts.Tenancy.Enums;

namespace FinancialProject.Domain.Contexts.Tenancy.Entities;

public class OrganizationSettings : TenantEntity<Guid>
{
    public string DefaultCurrencyCode { get; private set; } = "UZS";
    public ManagerVisibilityScope ManagerVisibilityScope { get; private set; } = ManagerVisibilityScope.OwnOnly;
    public decimal VatRate { get; private set; }
    public decimal ProfitTaxRate { get; private set; }
    public decimal DividendTaxRate { get; private set; }
    public decimal DefaultBankExpenseRate { get; private set; }
    public decimal DefaultRiskRate { get; private set; }
    public bool RequireFinanceApproval { get; private set; } = true;
    public bool RequireDirectorApproval { get; private set; } = true;

    private OrganizationSettings()
    {
    }

    public static OrganizationSettings Create(
        string defaultCurrencyCode,
        ManagerVisibilityScope managerVisibilityScope,
        decimal vatRate,
        decimal profitTaxRate,
        decimal dividendTaxRate,
        decimal defaultBankExpenseRate,
        decimal defaultRiskRate,
        bool requireFinanceApproval,
        bool requireDirectorApproval,
        Guid createdBy)
    {
        var settings = new OrganizationSettings
        {
            DefaultCurrencyCode = string.IsNullOrWhiteSpace(defaultCurrencyCode) ? "UZS" : defaultCurrencyCode.Trim().ToUpperInvariant(),
            ManagerVisibilityScope = managerVisibilityScope,
            VatRate = vatRate,
            ProfitTaxRate = profitTaxRate,
            DividendTaxRate = dividendTaxRate,
            DefaultBankExpenseRate = defaultBankExpenseRate,
            DefaultRiskRate = defaultRiskRate,
            RequireFinanceApproval = requireFinanceApproval,
            RequireDirectorApproval = requireDirectorApproval
        };

        settings.SetIdentity(Guid.NewGuid());
        settings.MarkCreated(createdBy, DateTimeOffset.UtcNow);
        return settings;
    }

    public void AttachToOrganization(Guid organizationId, Guid updatedBy)
    {
        SetOrganization(organizationId);
        MarkUpdated(updatedBy, DateTimeOffset.UtcNow);
    }
}
