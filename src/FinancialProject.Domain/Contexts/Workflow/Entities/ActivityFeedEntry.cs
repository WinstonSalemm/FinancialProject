using FinancialProject.Domain.Common;
using FinancialProject.Domain.Contexts.Workflow.Enums;

namespace FinancialProject.Domain.Contexts.Workflow.Entities;

public class ActivityFeedEntry : TenantEntity<Guid>, IFinancialAggregateRoot
{
    public Guid ActorMembershipId { get; private set; }
    public ActivityFeedType Type { get; private set; }
    public string Summary { get; private set; } = null!;
    public Guid? LeadId { get; private set; }
    public Guid? DealId { get; private set; }
    public Guid? ProjectId { get; private set; }
    public Guid? TaskId { get; private set; }
    public Guid? FinanceEntryId { get; private set; }
    public DateTimeOffset OccurredAt { get; private set; }

    private ActivityFeedEntry()
    {
    }

    public static ActivityFeedEntry Create(
        Guid organizationId,
        Guid actorMembershipId,
        ActivityFeedType type,
        string summary,
        Guid createdBy,
        Guid? leadId = null,
        Guid? dealId = null,
        Guid? projectId = null,
        Guid? taskId = null,
        Guid? financeEntryId = null)
    {
        var entity = new ActivityFeedEntry
        {
            ActorMembershipId = actorMembershipId,
            Type = type,
            Summary = string.IsNullOrWhiteSpace(summary) ? throw new ArgumentException("Activity summary is required.", nameof(summary)) : summary.Trim(),
            LeadId = leadId,
            DealId = dealId,
            ProjectId = projectId,
            TaskId = taskId,
            FinanceEntryId = financeEntryId,
            OccurredAt = DateTimeOffset.UtcNow
        };

        entity.SetIdentity(Guid.NewGuid());
        entity.SetOrganization(organizationId);
        entity.MarkCreated(createdBy, entity.OccurredAt);
        return entity;
    }
}
