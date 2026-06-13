using FinancialProject.Domain.Common;
using FinancialProject.Domain.Contexts.Workflow.Enums;

namespace FinancialProject.Domain.Contexts.Workflow.Entities;

public class FileAttachment : TenantSoftDeleteEntity<Guid>, IFinancialAggregateRoot
{
    public string OriginalFileName { get; private set; } = null!;
    public string StorageFileName { get; private set; } = null!;
    public string ContentType { get; private set; } = null!;
    public long SizeInBytes { get; private set; }
    public string StoragePath { get; private set; } = null!;
    public FileAttachmentKind Kind { get; private set; }
    public Guid? LeadId { get; private set; }
    public Guid? DealId { get; private set; }
    public Guid? ProjectId { get; private set; }
    public Guid? TaskId { get; private set; }
    public Guid? CommentId { get; private set; }
    public Guid? AuditLogId { get; private set; }
    public Guid? FinancialAdjustmentId { get; private set; }

    private FileAttachment()
    {
    }

    public static FileAttachment Create(
        Guid organizationId,
        string originalFileName,
        string storageFileName,
        string contentType,
        long sizeInBytes,
        string storagePath,
        FileAttachmentKind kind,
        Guid createdBy,
        Guid? leadId = null,
        Guid? dealId = null,
        Guid? projectId = null,
        Guid? taskId = null,
        Guid? commentId = null,
        Guid? auditLogId = null,
        Guid? financialAdjustmentId = null)
    {
        var entity = new FileAttachment
        {
            OriginalFileName = string.IsNullOrWhiteSpace(originalFileName) ? throw new ArgumentException("Original file name is required.", nameof(originalFileName)) : originalFileName.Trim(),
            StorageFileName = string.IsNullOrWhiteSpace(storageFileName) ? throw new ArgumentException("Storage file name is required.", nameof(storageFileName)) : storageFileName.Trim(),
            ContentType = string.IsNullOrWhiteSpace(contentType) ? "application/octet-stream" : contentType.Trim(),
            SizeInBytes = sizeInBytes,
            StoragePath = string.IsNullOrWhiteSpace(storagePath) ? throw new ArgumentException("Storage path is required.", nameof(storagePath)) : storagePath.Trim(),
            Kind = kind,
            LeadId = leadId,
            DealId = dealId,
            ProjectId = projectId,
            TaskId = taskId,
            CommentId = commentId,
            AuditLogId = auditLogId,
            FinancialAdjustmentId = financialAdjustmentId
        };

        entity.SetIdentity(Guid.NewGuid());
        entity.SetOrganization(organizationId);
        entity.MarkCreated(createdBy, DateTimeOffset.UtcNow);
        return entity;
    }
}
