using FinancialProject.Domain.Common;
using FinancialProject.Domain.Contexts.Workflow.Enums;
using WorkflowTaskStatus = FinancialProject.Domain.Contexts.Workflow.Enums.TaskStatus;

namespace FinancialProject.Domain.Contexts.Workflow.Entities;

public class TaskItem : TenantSoftDeleteEntity<Guid>, IFinancialAggregateRoot
{
    public Guid CreatedByMembershipId { get; private set; }
    public Guid? AssignedMembershipId { get; private set; }
    public Guid? AssignedDepartmentId { get; private set; }
    public Guid? LeadId { get; private set; }
    public Guid? DealId { get; private set; }
    public Guid? ProjectId { get; private set; }
    public Guid? ClientId { get; private set; }
    public Guid? TemplateId { get; private set; }
    public string Title { get; private set; } = null!;
    public string? Description { get; private set; }
    public WorkflowTaskStatus Status { get; private set; }
    public TaskPriority Priority { get; private set; }
    public DateTimeOffset DueAt { get; private set; }
    public string? ResultComment { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }
    public Guid? CompletedByMembershipId { get; private set; }

    private TaskItem()
    {
    }

    public static TaskItem Create(
        Guid organizationId,
        Guid createdByMembershipId,
        string title,
        string? description,
        TaskPriority priority,
        DateTimeOffset dueAt,
        Guid createdBy,
        Guid? assignedMembershipId = null,
        Guid? assignedDepartmentId = null,
        Guid? leadId = null,
        Guid? dealId = null,
        Guid? projectId = null,
        Guid? clientId = null,
        Guid? templateId = null)
    {
        var entity = new TaskItem
        {
            CreatedByMembershipId = createdByMembershipId,
            AssignedMembershipId = assignedMembershipId,
            AssignedDepartmentId = assignedDepartmentId,
            LeadId = leadId,
            DealId = dealId,
            ProjectId = projectId,
            ClientId = clientId,
            TemplateId = templateId,
            Title = string.IsNullOrWhiteSpace(title) ? throw new ArgumentException("Task title is required.", nameof(title)) : title.Trim(),
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            Status = WorkflowTaskStatus.New,
            Priority = priority,
            DueAt = dueAt
        };

        entity.SetIdentity(Guid.NewGuid());
        entity.SetOrganization(organizationId);
        entity.MarkCreated(createdBy, DateTimeOffset.UtcNow);
        return entity;
    }

    public void Start(Guid updatedBy)
    {
        Status = WorkflowTaskStatus.InProgress;
        MarkUpdated(updatedBy, DateTimeOffset.UtcNow);
    }

    public void Complete(Guid completedByMembershipId, string? resultComment, Guid updatedBy)
    {
        Status = WorkflowTaskStatus.Done;
        CompletedAt = DateTimeOffset.UtcNow;
        CompletedByMembershipId = completedByMembershipId;
        ResultComment = string.IsNullOrWhiteSpace(resultComment) ? null : resultComment.Trim();
        MarkUpdated(updatedBy, CompletedAt.Value);
    }
}
