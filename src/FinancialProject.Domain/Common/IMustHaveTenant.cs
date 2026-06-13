namespace FinancialProject.Domain.Common;

public interface IMustHaveTenant
{
    Guid OrganizationId { get; }
}
