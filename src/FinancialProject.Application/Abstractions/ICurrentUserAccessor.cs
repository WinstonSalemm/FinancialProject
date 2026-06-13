namespace FinancialProject.Application.Abstractions;

public interface ICurrentUserAccessor
{
    Guid? UserId { get; }
}
