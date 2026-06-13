using FinancialProject.Domain.Common;

namespace FinancialProject.Domain.Contexts.Workflow.Entities;

public class Comment : TenantSoftDeleteEntity<Guid>, IFinancialAggregateRoot
{
    private readonly List<CommentMention> _mentions = [];
    private readonly List<Comment> _replies = [];

    public Guid AuthorMembershipId { get; private set; }
    public Guid? LeadId { get; private set; }
    public Guid? DealId { get; private set; }
    public Guid? ProjectId { get; private set; }
    public Guid? TaskId { get; private set; }
    public Guid? ParentCommentId { get; private set; }
    public Comment? ParentComment { get; private set; }
    public string Body { get; private set; } = null!;
    public IReadOnlyCollection<CommentMention> Mentions => _mentions.AsReadOnly();
    public IReadOnlyCollection<Comment> Replies => _replies.AsReadOnly();

    private Comment()
    {
    }

    public static Comment Create(
        Guid organizationId,
        Guid authorMembershipId,
        string body,
        Guid createdBy,
        Guid? leadId = null,
        Guid? dealId = null,
        Guid? projectId = null,
        Guid? taskId = null,
        Guid? parentCommentId = null)
    {
        var entity = new Comment
        {
            AuthorMembershipId = authorMembershipId,
            LeadId = leadId,
            DealId = dealId,
            ProjectId = projectId,
            TaskId = taskId,
            ParentCommentId = parentCommentId,
            Body = string.IsNullOrWhiteSpace(body) ? throw new ArgumentException("Comment body is required.", nameof(body)) : body.Trim()
        };

        entity.SetIdentity(Guid.NewGuid());
        entity.SetOrganization(organizationId);
        entity.MarkCreated(createdBy, DateTimeOffset.UtcNow);
        return entity;
    }
}

public class CommentMention : TenantEntity<Guid>
{
    public Guid CommentId { get; private set; }
    public Comment Comment { get; private set; } = null!;
    public Guid MentionedMembershipId { get; private set; }

    private CommentMention()
    {
    }

    public static CommentMention Create(Guid organizationId, Guid commentId, Guid mentionedMembershipId, Guid createdBy)
    {
        var entity = new CommentMention
        {
            CommentId = commentId,
            MentionedMembershipId = mentionedMembershipId
        };

        entity.SetIdentity(Guid.NewGuid());
        entity.SetOrganization(organizationId);
        entity.MarkCreated(createdBy, DateTimeOffset.UtcNow);
        return entity;
    }
}
