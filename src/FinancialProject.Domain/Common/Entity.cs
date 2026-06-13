namespace FinancialProject.Domain.Common;

public abstract class Entity<TId>
{
    public TId Id { get; protected set; } = default!;
    public DateTimeOffset CreatedAt { get; protected set; }
    public Guid CreatedBy { get; protected set; }
    public DateTimeOffset? UpdatedAt { get; protected set; }
    public Guid? UpdatedBy { get; protected set; }

    protected void SetIdentity(TId id)
    {
        Id = id;
    }

    public virtual void MarkCreated(Guid userId, DateTimeOffset timestamp)
    {
        CreatedBy = userId;
        CreatedAt = timestamp;
        UpdatedBy = null;
        UpdatedAt = null;
    }

    public virtual void MarkUpdated(Guid userId, DateTimeOffset timestamp)
    {
        UpdatedBy = userId;
        UpdatedAt = timestamp;
    }
}

public abstract class TenantEntity<TId> : Entity<TId>, IMustHaveTenant
{
    public Guid OrganizationId { get; protected set; }

    protected void SetOrganization(Guid organizationId)
    {
        OrganizationId = organizationId;
    }
}

public abstract class SoftDeleteEntity<TId> : Entity<TId>, ISoftDelete
{
    public bool IsDeleted { get; protected set; }
    public DateTimeOffset? DeletedAt { get; protected set; }
    public Guid? DeletedBy { get; protected set; }

    public virtual void Delete(Guid userId, DateTimeOffset timestamp)
    {
        if (IsDeleted)
        {
            return;
        }

        IsDeleted = true;
        DeletedAt = timestamp;
        DeletedBy = userId;
        MarkUpdated(userId, timestamp);
    }

    public virtual void Restore(Guid userId, DateTimeOffset timestamp)
    {
        IsDeleted = false;
        DeletedAt = null;
        DeletedBy = null;
        MarkUpdated(userId, timestamp);
    }
}

public abstract class TenantSoftDeleteEntity<TId> : TenantEntity<TId>, ISoftDelete
{
    public bool IsDeleted { get; protected set; }
    public DateTimeOffset? DeletedAt { get; protected set; }
    public Guid? DeletedBy { get; protected set; }

    public virtual void Delete(Guid userId, DateTimeOffset timestamp)
    {
        if (IsDeleted)
        {
            return;
        }

        IsDeleted = true;
        DeletedAt = timestamp;
        DeletedBy = userId;
        MarkUpdated(userId, timestamp);
    }

    public virtual void Restore(Guid userId, DateTimeOffset timestamp)
    {
        IsDeleted = false;
        DeletedAt = null;
        DeletedBy = null;
        MarkUpdated(userId, timestamp);
    }
}
