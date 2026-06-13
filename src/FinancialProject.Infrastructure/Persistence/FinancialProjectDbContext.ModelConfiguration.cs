using FinancialProject.Domain.Common;
using FinancialProject.Domain.Contexts.Clients.Entities;
using FinancialProject.Domain.Contexts.Finance.Entities;
using FinancialProject.Domain.Contexts.Identity.Entities;
using FinancialProject.Domain.Contexts.Sales.Entities;
using FinancialProject.Domain.Contexts.Tenancy.Entities;
using FinancialProject.Domain.Contexts.Workflow.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialProject.Infrastructure.Persistence;

public sealed partial class FinancialProjectDbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureTenancy(modelBuilder);
        ConfigureIdentity(modelBuilder);
        ConfigureClients(modelBuilder);
        ConfigureSales(modelBuilder);
        ConfigureFinance(modelBuilder);
        ConfigureWorkflow(modelBuilder);
    }

    private void ConfigureTenancy(ModelBuilder modelBuilder)
    {
        var holdings = modelBuilder.Entity<Holding>();
        holdings.ToTable("holdings");
        holdings.Property(x => x.Name).HasMaxLength(256).IsRequired();
        holdings.Property(x => x.Description).HasMaxLength(1000);
        holdings.HasIndex(x => x.Name).IsUnique();
        ApplySoftDeleteFilter(holdings);

        var organizations = modelBuilder.Entity<Organization>();
        organizations.ToTable("organizations");
        organizations.Property(x => x.Name).HasMaxLength(256).IsRequired();
        organizations.Property(x => x.Tin).HasMaxLength(64).IsRequired();
        organizations.Property(x => x.Phone).HasMaxLength(64).IsRequired();
        organizations.HasOne(x => x.Holding).WithMany(x => x.Organizations).HasForeignKey(x => x.HoldingId).OnDelete(DeleteBehavior.Restrict);
        organizations.HasOne(x => x.Settings).WithOne().HasForeignKey<OrganizationSettings>(x => x.OrganizationId).OnDelete(DeleteBehavior.Cascade);
        organizations.HasIndex(x => new { x.Name, x.Tin }).IsUnique();
        ApplySoftDeleteFilter(organizations);

        var organizationSettings = modelBuilder.Entity<OrganizationSettings>();
        organizationSettings.ToTable("organization_settings");
        organizationSettings.Property(x => x.DefaultCurrencyCode).HasMaxLength(8).IsRequired();
        ConfigureTenantEntity(organizationSettings);
        ApplyTenantFilter(organizationSettings);
    }

    private void ConfigureIdentity(ModelBuilder modelBuilder)
    {
        var users = modelBuilder.Entity<User>();
        users.ToTable("users");
        users.Property(x => x.FullName).HasMaxLength(256).IsRequired();
        users.Property(x => x.Email).HasMaxLength(320).IsRequired();
        users.Property(x => x.PhoneNumber).HasMaxLength(64);
        users.Property(x => x.PasswordHash).HasMaxLength(512).IsRequired();
        users.HasIndex(x => x.Email).IsUnique();
        ApplySoftDeleteFilter(users);

        var memberships = modelBuilder.Entity<OrganizationMembership>();
        memberships.ToTable("organization_memberships");
        memberships.Property(x => x.PositionTitle).HasMaxLength(256).IsRequired();
        memberships.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        memberships.HasIndex(x => new { x.OrganizationId, x.UserId }).IsUnique();
        ConfigureTenantSoftDeleteEntity(memberships);

        var departments = modelBuilder.Entity<Department>();
        departments.ToTable("departments");
        departments.Property(x => x.Code).HasMaxLength(64).IsRequired();
        departments.Property(x => x.Name).HasMaxLength(128).IsRequired();
        departments.Property(x => x.Description).HasMaxLength(1000);
        departments.HasIndex(x => new { x.OrganizationId, x.Code }).IsUnique();
        ConfigureTenantSoftDeleteEntity(departments);

        var roles = modelBuilder.Entity<Role>();
        roles.ToTable("roles");
        roles.Property(x => x.Code).HasMaxLength(64).IsRequired();
        roles.Property(x => x.Name).HasMaxLength(128).IsRequired();
        roles.Property(x => x.Description).HasMaxLength(1000);
        roles.HasIndex(x => new { x.OrganizationId, x.Code }).IsUnique();
        ConfigureTenantSoftDeleteEntity(roles);

        var membershipRoles = modelBuilder.Entity<OrganizationMembershipRole>();
        membershipRoles.ToTable("organization_membership_roles");
        membershipRoles.HasOne(x => x.Membership).WithMany(x => x.Roles).HasForeignKey(x => x.MembershipId).OnDelete(DeleteBehavior.Cascade);
        membershipRoles.HasOne(x => x.Role).WithMany().HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Restrict);
        membershipRoles.HasIndex(x => new { x.OrganizationId, x.MembershipId, x.RoleId }).IsUnique();
        ConfigureTenantEntity(membershipRoles);
        ApplyTenantFilter(membershipRoles);

        var membershipDepartments = modelBuilder.Entity<OrganizationMembershipDepartment>();
        membershipDepartments.ToTable("organization_membership_departments");
        membershipDepartments.HasOne(x => x.Membership).WithMany(x => x.Departments).HasForeignKey(x => x.MembershipId).OnDelete(DeleteBehavior.Cascade);
        membershipDepartments.HasOne(x => x.Department).WithMany().HasForeignKey(x => x.DepartmentId).OnDelete(DeleteBehavior.Restrict);
        membershipDepartments.HasIndex(x => new { x.OrganizationId, x.MembershipId, x.DepartmentId }).IsUnique();
        ConfigureTenantEntity(membershipDepartments);
        ApplyTenantFilter(membershipDepartments);
    }

    private void ConfigureClients(ModelBuilder modelBuilder)
    {
        var clients = modelBuilder.Entity<Client>();
        clients.ToTable("clients");
        clients.Property(x => x.CompanyName).HasMaxLength(256).IsRequired();
        clients.Property(x => x.Tin).HasMaxLength(64).IsRequired();
        clients.Property(x => x.Phone).HasMaxLength(64).IsRequired();
        clients.Property(x => x.Complexity).HasMaxLength(256);
        clients.Property(x => x.Notes).HasMaxLength(4000);
        clients.Property(x => x.Features).HasMaxLength(2000);
        clients.HasIndex(x => new { x.OrganizationId, x.Tin }).IsUnique();
        ConfigureTenantSoftDeleteEntity(clients);
    }

    private void ConfigureSales(ModelBuilder modelBuilder)
    {
        var leads = modelBuilder.Entity<Lead>();
        leads.ToTable("leads");
        leads.Property(x => x.Title).HasMaxLength(256).IsRequired();
        leads.Property(x => x.Description).HasMaxLength(4000);
        leads.Property(x => x.RejectionReason).HasMaxLength(1000);
        leads.Property(x => x.RejectionComment).HasMaxLength(4000);
        leads.HasIndex(x => new { x.OrganizationId, x.OwnerMembershipId, x.Status });
        ConfigureTenantSoftDeleteEntity(leads);

        var dealLeadDetails = modelBuilder.Entity<DealLeadDetails>();
        dealLeadDetails.ToTable("deal_lead_details");
        dealLeadDetails.HasOne(x => x.Lead).WithOne(x => x.DealDetails).HasForeignKey<DealLeadDetails>(x => x.LeadId).OnDelete(DeleteBehavior.Cascade);
        dealLeadDetails.HasIndex(x => new { x.OrganizationId, x.LeadId }).IsUnique();
        ConfigureTenantEntity(dealLeadDetails);
        ApplyTenantFilter(dealLeadDetails);

        var dealLeadItems = modelBuilder.Entity<DealLeadItem>();
        dealLeadItems.ToTable("deal_lead_items");
        dealLeadItems.Property(x => x.PartNumber).HasMaxLength(128);
        dealLeadItems.Property(x => x.Description).HasMaxLength(2000).IsRequired();
        dealLeadItems.HasOne(x => x.DealLeadDetails).WithMany(x => x.Items).HasForeignKey(x => x.DealLeadDetailsId).OnDelete(DeleteBehavior.Cascade);
        ConfigureTenantEntity(dealLeadItems);
        ApplyTenantFilter(dealLeadItems);

        var projectLeadDetails = modelBuilder.Entity<ProjectLeadDetails>();
        projectLeadDetails.ToTable("project_lead_details");
        projectLeadDetails.HasOne(x => x.Lead).WithOne(x => x.ProjectDetails).HasForeignKey<ProjectLeadDetails>(x => x.LeadId).OnDelete(DeleteBehavior.Cascade);
        projectLeadDetails.HasIndex(x => new { x.OrganizationId, x.LeadId }).IsUnique();
        ConfigureTenantEntity(projectLeadDetails);
        ApplyTenantFilter(projectLeadDetails);

        var leadCostLines = modelBuilder.Entity<LeadCostLine>();
        leadCostLines.ToTable("lead_cost_lines");
        leadCostLines.Property(x => x.Description).HasMaxLength(1000).IsRequired();
        leadCostLines.Property(x => x.PerformerName).HasMaxLength(256);
        leadCostLines.HasOne(x => x.DealLeadDetails).WithMany(x => x.CostLines).HasForeignKey(x => x.DealLeadDetailsId).OnDelete(DeleteBehavior.Cascade);
        leadCostLines.HasOne(x => x.ProjectLeadDetails).WithMany(x => x.CostLines).HasForeignKey(x => x.ProjectLeadDetailsId).OnDelete(DeleteBehavior.Cascade);
        leadCostLines.ToTable(table => table.HasCheckConstraint("ck_lead_cost_lines_owner", "(deal_lead_details_id IS NOT NULL) <> (project_lead_details_id IS NOT NULL)"));
        ConfigureTenantEntity(leadCostLines);
        ApplyTenantFilter(leadCostLines);

        var leadApprovals = modelBuilder.Entity<LeadApprovalRecord>();
        leadApprovals.ToTable("lead_approval_records");
        leadApprovals.Property(x => x.Comment).HasMaxLength(4000);
        leadApprovals.Property(x => x.Reason).HasMaxLength(2000);
        leadApprovals.HasOne(x => x.Lead).WithMany(x => x.ApprovalHistory).HasForeignKey(x => x.LeadId).OnDelete(DeleteBehavior.Cascade);
        ConfigureTenantEntity(leadApprovals);
        ApplyTenantFilter(leadApprovals);

        var privateNotes = modelBuilder.Entity<PrivateNote>();
        privateNotes.ToTable("private_notes");
        privateNotes.Property(x => x.Body).HasMaxLength(4000).IsRequired();
        privateNotes.ToTable(table => table.HasCheckConstraint("ck_private_notes_owner", "((lead_id IS NOT NULL)::int + (deal_id IS NOT NULL)::int + (project_id IS NOT NULL)::int) = 1"));
        ConfigureTenantEntity(privateNotes);
        ApplyTenantFilter(privateNotes);

        var deals = modelBuilder.Entity<Deal>();
        deals.ToTable("deals");
        deals.Property(x => x.Title).HasMaxLength(256).IsRequired();
        deals.Property(x => x.ContractNumber).HasMaxLength(256);
        deals.HasIndex(x => new { x.OrganizationId, x.OwnerMembershipId, x.Status });
        ConfigureTenantSoftDeleteEntity(deals);

        var dealItems = modelBuilder.Entity<DealItem>();
        dealItems.ToTable("deal_items");
        dealItems.Property(x => x.PartNumber).HasMaxLength(128);
        dealItems.Property(x => x.Description).HasMaxLength(2000).IsRequired();
        dealItems.HasOne(x => x.Deal).WithMany(x => x.Items).HasForeignKey(x => x.DealId).OnDelete(DeleteBehavior.Cascade);
        ConfigureTenantEntity(dealItems);
        ApplyTenantFilter(dealItems);

        var dealCostLines = modelBuilder.Entity<DealCostLine>();
        dealCostLines.ToTable("deal_cost_lines");
        dealCostLines.Property(x => x.Description).HasMaxLength(1000).IsRequired();
        dealCostLines.Property(x => x.PerformerName).HasMaxLength(256);
        dealCostLines.HasOne(x => x.Deal).WithMany(x => x.CostLines).HasForeignKey(x => x.DealId).OnDelete(DeleteBehavior.Cascade);
        ConfigureTenantEntity(dealCostLines);
        ApplyTenantFilter(dealCostLines);

        var dealObligations = modelBuilder.Entity<DealObligation>();
        dealObligations.ToTable("deal_obligations");
        dealObligations.Property(x => x.Title).HasMaxLength(256).IsRequired();
        dealObligations.Property(x => x.Description).HasMaxLength(4000);
        dealObligations.HasOne(x => x.Deal).WithMany(x => x.Obligations).HasForeignKey(x => x.DealId).OnDelete(DeleteBehavior.Cascade);
        ConfigureTenantEntity(dealObligations);
        ApplyTenantFilter(dealObligations);

        var projects = modelBuilder.Entity<Project>();
        projects.ToTable("projects");
        projects.Property(x => x.Title).HasMaxLength(256).IsRequired();
        projects.Property(x => x.ContractNumber).HasMaxLength(256);
        projects.HasIndex(x => new { x.OrganizationId, x.OwnerMembershipId, x.Status });
        ConfigureTenantSoftDeleteEntity(projects);

        var projectCostLines = modelBuilder.Entity<ProjectCostLine>();
        projectCostLines.ToTable("project_cost_lines");
        projectCostLines.Property(x => x.Description).HasMaxLength(1000).IsRequired();
        projectCostLines.Property(x => x.PerformerName).HasMaxLength(256);
        projectCostLines.HasOne(x => x.Project).WithMany(x => x.CostLines).HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Cascade);
        ConfigureTenantEntity(projectCostLines);
        ApplyTenantFilter(projectCostLines);
    }

    private void ConfigureFinance(ModelBuilder modelBuilder)
    {
        var financeEntries = modelBuilder.Entity<FinanceEntry>();
        financeEntries.ToTable("finance_entries");
        financeEntries.Property(x => x.Description).HasMaxLength(1000).IsRequired();
        financeEntries.Property(x => x.CurrencyCode).HasMaxLength(8).IsRequired();
        financeEntries.Property(x => x.ExternalReference).HasMaxLength(256);
        financeEntries.HasIndex(x => new { x.OrganizationId, x.OccurredOn, x.Type, x.Direction });
        ConfigureTenantSoftDeleteEntity(financeEntries);

        var adminExpenseCategories = modelBuilder.Entity<AdminExpenseCategory>();
        adminExpenseCategories.ToTable("admin_expense_categories");
        adminExpenseCategories.Property(x => x.Code).HasMaxLength(64).IsRequired();
        adminExpenseCategories.Property(x => x.Name).HasMaxLength(128).IsRequired();
        adminExpenseCategories.Property(x => x.Description).HasMaxLength(1000);
        adminExpenseCategories.HasIndex(x => new { x.OrganizationId, x.Code }).IsUnique();
        ConfigureTenantSoftDeleteEntity(adminExpenseCategories);

        var adminExpenses = modelBuilder.Entity<AdminExpense>();
        adminExpenses.ToTable("admin_expenses");
        adminExpenses.Property(x => x.Title).HasMaxLength(256).IsRequired();
        adminExpenses.Property(x => x.Notes).HasMaxLength(4000);
        adminExpenses.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
        adminExpenses.HasIndex(x => new { x.OrganizationId, x.PeriodYear, x.PeriodMonth, x.Status });
        ConfigureTenantSoftDeleteEntity(adminExpenses);

        var taxRates = modelBuilder.Entity<TaxRate>();
        taxRates.ToTable("tax_rates");
        taxRates.Property(x => x.Name).HasMaxLength(128).IsRequired();
        taxRates.HasIndex(x => new { x.OrganizationId, x.Type, x.EffectiveFrom });
        ConfigureTenantSoftDeleteEntity(taxRates);

        var financialAdjustments = modelBuilder.Entity<FinancialAdjustment>();
        financialAdjustments.ToTable("financial_adjustments");
        financialAdjustments.Property(x => x.Reason).HasMaxLength(1000).IsRequired();
        financialAdjustments.Property(x => x.Comment).HasMaxLength(4000);
        ConfigureTenantSoftDeleteEntity(financialAdjustments);
    }

    private void ConfigureWorkflow(ModelBuilder modelBuilder)
    {
        var taskItems = modelBuilder.Entity<TaskItem>();
        taskItems.ToTable("tasks");
        taskItems.Property(x => x.Title).HasMaxLength(256).IsRequired();
        taskItems.Property(x => x.Description).HasMaxLength(4000);
        taskItems.Property(x => x.ResultComment).HasMaxLength(4000);
        taskItems.HasIndex(x => new { x.OrganizationId, x.AssignedMembershipId, x.Status, x.DueAt });
        ConfigureTenantSoftDeleteEntity(taskItems);

        var comments = modelBuilder.Entity<Comment>();
        comments.ToTable("comments");
        comments.Property(x => x.Body).HasMaxLength(4000).IsRequired();
        comments.HasOne(x => x.ParentComment).WithMany(x => x.Replies).HasForeignKey(x => x.ParentCommentId).OnDelete(DeleteBehavior.Restrict);
        comments.ToTable(table => table.HasCheckConstraint("ck_comments_owner", "((lead_id IS NOT NULL)::int + (deal_id IS NOT NULL)::int + (project_id IS NOT NULL)::int + (task_id IS NOT NULL)::int) = 1"));
        ConfigureTenantSoftDeleteEntity(comments);

        var commentMentions = modelBuilder.Entity<CommentMention>();
        commentMentions.ToTable("comment_mentions");
        commentMentions.HasOne(x => x.Comment).WithMany(x => x.Mentions).HasForeignKey(x => x.CommentId).OnDelete(DeleteBehavior.Cascade);
        commentMentions.HasIndex(x => new { x.OrganizationId, x.CommentId, x.MentionedMembershipId }).IsUnique();
        ConfigureTenantEntity(commentMentions);
        ApplyTenantFilter(commentMentions);

        var notifications = modelBuilder.Entity<Notification>();
        notifications.ToTable("notifications");
        notifications.Property(x => x.Title).HasMaxLength(256).IsRequired();
        notifications.Property(x => x.Body).HasMaxLength(4000).IsRequired();
        notifications.HasIndex(x => new { x.OrganizationId, x.MembershipId, x.Status, x.CreatedAt });
        ConfigureTenantSoftDeleteEntity(notifications);

        var activityFeedEntries = modelBuilder.Entity<ActivityFeedEntry>();
        activityFeedEntries.ToTable("activity_feed_entries");
        activityFeedEntries.Property(x => x.Summary).HasMaxLength(1000).IsRequired();
        activityFeedEntries.HasIndex(x => new { x.OrganizationId, x.OccurredAt });
        ConfigureTenantEntity(activityFeedEntries);
        ApplyTenantFilter(activityFeedEntries);

        var fileAttachments = modelBuilder.Entity<FileAttachment>();
        fileAttachments.ToTable("file_attachments");
        fileAttachments.Property(x => x.OriginalFileName).HasMaxLength(260).IsRequired();
        fileAttachments.Property(x => x.StorageFileName).HasMaxLength(260).IsRequired();
        fileAttachments.Property(x => x.ContentType).HasMaxLength(128).IsRequired();
        fileAttachments.Property(x => x.StoragePath).HasMaxLength(512).IsRequired();
        ConfigureTenantSoftDeleteEntity(fileAttachments);

        var auditLogs = modelBuilder.Entity<AuditLog>();
        auditLogs.ToTable("audit_logs");
        auditLogs.Property(x => x.EntityName).HasMaxLength(256).IsRequired();
        auditLogs.Property(x => x.FieldName).HasMaxLength(256);
        auditLogs.Property(x => x.OldValue).HasMaxLength(4000);
        auditLogs.Property(x => x.NewValue).HasMaxLength(4000);
        auditLogs.Property(x => x.Reason).HasMaxLength(2000);
        auditLogs.HasIndex(x => new { x.OrganizationId, x.EntityName, x.EntityIdValue, x.ChangedAt });
        ConfigureTenantEntity(auditLogs);
        ApplyTenantFilter(auditLogs);

        var templates = modelBuilder.Entity<Template>();
        templates.ToTable("templates");
        templates.Property(x => x.Name).HasMaxLength(256).IsRequired();
        templates.Property(x => x.Description).HasMaxLength(4000);
        templates.Property(x => x.StructureJson).HasColumnType("jsonb").IsRequired();
        templates.Property(x => x.TypicalRisksJson).HasColumnType("jsonb");
        templates.Property(x => x.TypicalExpensesJson).HasColumnType("jsonb");
        ConfigureTenantSoftDeleteEntity(templates);
    }

    private static void ConfigureTenantEntity<TEntity>(EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IMustHaveTenant
    {
        builder.Property(x => x.OrganizationId).IsRequired();
        builder.HasIndex(x => x.OrganizationId);
    }

    private void ConfigureTenantSoftDeleteEntity<TEntity>(EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IMustHaveTenant, ISoftDelete
    {
        ConfigureTenantEntity(builder);
        ApplyTenantSoftDeleteFilter(builder);
    }

    private void ApplyTenantFilter<TEntity>(EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IMustHaveTenant
    {
        builder.HasQueryFilter(x => !CurrentOrganizationId.HasValue || x.OrganizationId == CurrentOrganizationId.Value);
    }

    private void ApplyTenantSoftDeleteFilter<TEntity>(EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IMustHaveTenant, ISoftDelete
    {
        builder.HasQueryFilter(x => (!CurrentOrganizationId.HasValue || x.OrganizationId == CurrentOrganizationId.Value) && !x.IsDeleted);
    }

    private void ApplySoftDeleteFilter<TEntity>(EntityTypeBuilder<TEntity> builder)
        where TEntity : class, ISoftDelete
    {
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
