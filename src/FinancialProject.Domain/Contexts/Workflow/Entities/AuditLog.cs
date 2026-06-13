using FinancialProject.Domain.Common;
using FinancialProject.Domain.Contexts.Workflow.Enums;

namespace FinancialProject.Domain.Contexts.Workflow.Entities;

public class AuditLog : TenantEntity<Guid>, IFinancialAggregateRoot
{
    public AuditActionType ActionType { get; private set; }
    public string EntityName { get; private set; } = null!;
    public Guid EntityIdValue { get; private set; }
    public string? FieldName { get; private set; }
    public string? OldValue { get; private set; }
    public string? NewValue { get; private set; }
    public string? Reason { get; private set; }
    public Guid ChangedByMembershipId { get; private set; }
    public DateTimeOffset ChangedAt { get; private set; }

    private AuditLog()
    {
    }

    public static AuditLog Create(
        Guid organizationId,
        AuditActionType actionType,
        string entityName,
        Guid entityIdValue,
        Guid changedByMembershipId,
        Guid createdBy,
        string? fieldName = null,
        string? oldValue = null,
        string? newValue = null,
        string? reason = null)
    {
        var entity = new AuditLog
        {
            ActionType = actionType,
            EntityName = string.IsNullOrWhiteSpace(entityName) ? throw new ArgumentException("Audit entity name is required.", nameof(entityName)) : entityName.Trim(),
            EntityIdValue = entityIdValue,
            FieldName = string.IsNullOrWhiteSpace(fieldName) ? null : fieldName.Trim(),
            OldValue = string.IsNullOrWhiteSpace(oldValue) ? null : oldValue.Trim(),
            NewValue = string.IsNullOrWhiteSpace(newValue) ? null : newValue.Trim(),
            Reason = string.IsNullOrWhiteSpace(reason) ? null : reason.Trim(),
            ChangedByMembershipId = changedByMembershipId,
            ChangedAt = DateTimeOffset.UtcNow
        };

        entity.SetIdentity(Guid.NewGuid());
        entity.SetOrganization(organizationId);
        entity.MarkCreated(createdBy, entity.ChangedAt);
        return entity;
    }
}
