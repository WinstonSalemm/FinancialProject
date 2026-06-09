using Microsoft.EntityFrameworkCore;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();

    public DbSet<Client> Clients => Set<Client>();

    public DbSet<Lead> Leads => Set<Lead>();
    public DbSet<LeadHistory> LeadHistories => Set<LeadHistory>();

    public DbSet<Deal> Deals => Set<Deal>();
    public DbSet<DealHistory> DealHistories => Set<DealHistory>();

    public DbSet<Calculation> Calculations => Set<Calculation>();
    public DbSet<CalculationVersion> CalculationVersions => Set<CalculationVersion>();

    public DbSet<ApprovalRequest> ApprovalRequests => Set<ApprovalRequest>();

    public DbSet<Department> Departments => Set<Department>();
    public DbSet<UserDepartment> UserDepartments => Set<UserDepartment>();

    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<LeadPrivateNote> LeadPrivateNotes => Set<LeadPrivateNote>();

    public DbSet<DealPrivateNote> DealPrivateNotes => Set<DealPrivateNote>();

    public DbSet<DealComment> DealComments => Set<DealComment>();

    public DbSet<CalculationVersionItem> CalculationVersionItems
        => Set<CalculationVersionItem>();

    public DbSet<CalculationResult> CalculationResults
        => Set<CalculationResult>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);
    }
}

// Use the CalculationResult type from the domain project. Removed local duplicate type.