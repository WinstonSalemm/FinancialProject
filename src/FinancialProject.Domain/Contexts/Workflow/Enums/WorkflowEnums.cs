namespace FinancialProject.Domain.Contexts.Workflow.Enums;

public enum TaskStatus
{
    New = 1,
    InProgress = 2,
    Waiting = 3,
    Blocked = 4,
    Done = 5,
    Cancelled = 6
}

public enum TaskPriority
{
    Low = 1,
    Medium = 2,
    High = 3,
    Urgent = 4
}

public enum NotificationType
{
    Lead = 1,
    Deal = 2,
    Project = 3,
    Task = 4,
    Comment = 5,
    System = 6
}

public enum NotificationStatus
{
    Unread = 1,
    Read = 2
}

public enum ActivityFeedType
{
    LeadCreated = 1,
    LeadApproved = 2,
    LeadRejected = 3,
    DealCreated = 4,
    DealCompleted = 5,
    ProjectCreated = 6,
    ProjectCompleted = 7,
    FinanceChanged = 8,
    TaskCreated = 9,
    TaskCompleted = 10,
    CommentAdded = 11,
    StatusChanged = 12
}

public enum AuditActionType
{
    Created = 1,
    Updated = 2,
    StatusChanged = 3,
    Deleted = 4,
    Restored = 5,
    Approved = 6,
    Rejected = 7
}

public enum FileAttachmentKind
{
    Generic = 1,
    BasisDocument = 2,
    Contract = 3,
    Invoice = 4,
    ApprovalFile = 5,
    CommentAttachment = 6
}

public enum TemplateType
{
    Deal = 1,
    Project = 2,
    Task = 3
}
