using FinancialProject.Application.Abstractions;
using FinancialProject.Domain.Common;
using FinancialProject.Domain.Contexts.Clients.Entities;
using FinancialProject.Domain.Contexts.Finance.Entities;
using FinancialProject.Domain.Contexts.Identity.Entities;
using FinancialProject.Domain.Contexts.Sales.Entities;
using FinancialProject.Domain.Contexts.Tenancy.Entities;
using FinancialProject.Domain.Contexts.Workflow.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancialProject.Infrastructure.Persistence;

public sealed partial class FinancialProjectDbContext : DbContext
{
    private readonly ICurrentTenantAccessor _currentTenantAccessor;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public FinancialProjectDbContext(
        DbContextOptions<FinancialProjectDbContext> options,
        ICurrentTenantAccessor currentTenantAccessor,
        ICurrentUserAccessor currentUserAccessor)
        : base(options)
    {
        _currentTenantAccessor = currentTenantAccessor;
        _currentUserAccessor = currentUserAccessor;
    }

    public Guid? CurrentOrganizationId => _currentTenantAccessor.OrganizationId;

    public DbSet<Holding> Holdings => Set<Holding>();
    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<OrganizationSettings> OrganizationSettings => Set<OrganizationSettings>();
    public DbSet<User> Users => Set<User>();
    public DbSet<OrganizationMembership> OrganizationMemberships => Set<OrganizationMembership>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<OrganizationMembershipRole> OrganizationMembershipRoles => Set<OrganizationMembershipRole>();
    public DbSet<OrganizationMembershipDepartment> OrganizationMembershipDepartments => Set<OrganizationMembershipDepartment>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Lead> Leads => Set<Lead>();
    public DbSet<DealLeadDetails> DealLeadDetails => Set<DealLeadDetails>();
    public DbSet<DealLeadItem> DealLeadItems => Set<DealLeadItem>();
    public DbSet<ProjectLeadDetails> ProjectLeadDetails => Set<ProjectLeadDetails>();
    public DbSet<LeadCostLine> LeadCostLines => Set<LeadCostLine>();
    public DbSet<LeadApprovalRecord> LeadApprovalRecords => Set<LeadApprovalRecord>();
    public DbSet<PrivateNote> PrivateNotes => Set<PrivateNote>();
    public DbSet<Deal> Deals => Set<Deal>();
    public DbSet<DealItem> DealItems => Set<DealItem>();
    public DbSet<DealCostLine> DealCostLines => Set<DealCostLine>();
    public DbSet<DealObligation> DealObligations => Set<DealObligation>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectCostLine> ProjectCostLines => Set<ProjectCostLine>();
    public DbSet<FinanceEntry> FinanceEntries => Set<FinanceEntry>();
    public DbSet<AdminExpenseCategory> AdminExpenseCategories => Set<AdminExpenseCategory>();
    public DbSet<AdminExpense> AdminExpenses => Set<AdminExpense>();
    public DbSet<TaxRate> TaxRates => Set<TaxRate>();
    public DbSet<FinancialAdjustment> FinancialAdjustments => Set<FinancialAdjustment>();
    public DbSet<TaskItem> TaskItems => Set<TaskItem>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<CommentMention> CommentMentions => Set<CommentMention>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<ActivityFeedEntry> ActivityFeedEntries => Set<ActivityFeedEntry>();
    public DbSet<FileAttachment> FileAttachments => Set<FileAttachment>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<Template> Templates => Set<Template>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>().HavePrecision(18, 4);
        configurationBuilder.Properties<decimal?>().HavePrecision(18, 4);
        configurationBuilder.Properties<DateOnly>().HaveConversion<DateOnlyConverter>();
        configurationBuilder.Properties<DateOnly?>().HaveConversion<NullableDateOnlyConverter>();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditMetadata();
        return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        ApplyAuditMetadata();
        return base.SaveChanges();
    }

    private void ApplyAuditMetadata()
    {
        var now = DateTimeOffset.UtcNow;
        var userId = _currentUserAccessor.UserId ?? Guid.Empty;

        foreach (var entry in ChangeTracker.Entries<Entity<Guid>>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.MarkCreated(userId, now);
                    break;

                case EntityState.Modified:
                    entry.Entity.MarkUpdated(userId, now);
                    break;

                case EntityState.Deleted:
                    if (entry.Entity is SoftDeleteEntity<Guid> softDeleteEntity)
                    {
                        entry.State = EntityState.Modified;
                        softDeleteEntity.Delete(userId, now);
                    }
                    else if (entry.Entity is TenantSoftDeleteEntity<Guid> tenantSoftDeleteEntity)
                    {
                        entry.State = EntityState.Modified;
                        tenantSoftDeleteEntity.Delete(userId, now);
                    }
                    break;
            }
        }
    }
}

internal sealed class DateOnlyConverter : Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateOnly, DateTime>
{
    public DateOnlyConverter()
        : base(
            value => value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc),
            value => DateOnly.FromDateTime(DateTime.SpecifyKind(value, DateTimeKind.Utc)))
    {
    }
}

internal sealed class NullableDateOnlyConverter : Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateOnly?, DateTime?>
{
    public NullableDateOnlyConverter()
        : base(
            value => value.HasValue ? value.Value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc) : null,
            value => value.HasValue ? DateOnly.FromDateTime(DateTime.SpecifyKind(value.Value, DateTimeKind.Utc)) : null)
    {
    }
}
