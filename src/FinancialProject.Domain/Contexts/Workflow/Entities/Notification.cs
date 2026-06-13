using FinancialProject.Domain.Common;
using FinancialProject.Domain.Contexts.Workflow.Enums;

namespace FinancialProject.Domain.Contexts.Workflow.Entities;

public class Notification : TenantSoftDeleteEntity<Guid>, IFinancialAggregateRoot
{
    public Guid MembershipId { get; private set; }
    public NotificationType Type { get; private set; }
    public NotificationStatus Status { get; private set; }
    public string Title { get; private set; } = null!;
    public string Body { get; private set; } = null!;
    public Guid? LeadId { get; private set; }
    public Guid? DealId { get; private set; }
    public Guid? ProjectId { get; private set; }
    public Guid? TaskId { get; private set; }
    public Guid? CommentId { get; private set; }
    public DateTimeOffset? ReadAt { get; private set; }

    private Notification()
    {
    }

    public static Notification Create(
        Guid organizationId,
        Guid membershipId,
        NotificationType type,
        string title,
        string body,
        Guid createdBy,
        Guid? leadId = null,
        Guid? dealId = null,
        Guid? projectId = null,
        Guid? taskId = null,
        Guid? commentId = null)
    {
        var entity = new Notification
        {
            MembershipId = membershipId,
            Type = type,
            Status = NotificationStatus.Unread,
            Title = string.IsNullOrWhiteSpace(title) ? throw new ArgumentException("Notification title is required.", nameof(title)) : title.Trim(),
            Body = string.IsNullOrWhiteSpace(body) ? throw new ArgumentException("Notification body is required.", nameof(body)) : body.Trim(),
            LeadId = leadId,
            DealId = dealId,
            ProjectId = projectId,
            TaskId = taskId,
            CommentId = commentId
        };

        entity.SetIdentity(Guid.NewGuid());
        entity.SetOrganization(organizationId);
        entity.MarkCreated(createdBy, DateTimeOffset.UtcNow);
        return entity;
    }

    public void MarkAsRead(Guid updatedBy)
    {
        Status = NotificationStatus.Read;
        ReadAt = DateTimeOffset.UtcNow;
        MarkUpdated(updatedBy, ReadAt.Value);
    }
}
