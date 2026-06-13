namespace FinancialProject.Application.Abstractions;

public interface ICurrentTenantAccessor
{
    Guid? OrganizationId { get; }
}
