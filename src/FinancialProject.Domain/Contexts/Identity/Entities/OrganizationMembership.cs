using FinancialProject.Domain.Common;

namespace FinancialProject.Domain.Contexts.Identity.Entities;

public class OrganizationMembership : TenantSoftDeleteEntity<Guid>, IFinancialAggregateRoot
{
    private readonly List<OrganizationMembershipRole> _roles = [];
    private readonly List<OrganizationMembershipDepartment> _departments = [];

    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    public string PositionTitle { get; private set; } = null!;
    public DateTimeOffset? LastLoginAt { get; private set; }
    public bool IsActive { get; private set; } = true;
    public IReadOnlyCollection<OrganizationMembershipRole> Roles => _roles.AsReadOnly();
    public IReadOnlyCollection<OrganizationMembershipDepartment> Departments => _departments.AsReadOnly();

    private OrganizationMembership()
    {
    }

    public static OrganizationMembership Create(Guid organizationId, Guid userId, string positionTitle, Guid createdBy)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User id is required.", nameof(userId));
        }

        var entity = new OrganizationMembership
        {
            UserId = userId,
            PositionTitle = string.IsNullOrWhiteSpace(positionTitle) ? "Employee" : positionTitle.Trim()
        };

        entity.SetIdentity(Guid.NewGuid());
        entity.SetOrganization(organizationId);
        entity.MarkCreated(createdBy, DateTimeOffset.UtcNow);
        return entity;
    }

    public void UpdatePosition(string positionTitle, Guid updatedBy)
    {
        PositionTitle = string.IsNullOrWhiteSpace(positionTitle) ? "Employee" : positionTitle.Trim();
        MarkUpdated(updatedBy, DateTimeOffset.UtcNow);
    }

    public void SetActivity(bool isActive, Guid updatedBy)
    {
        IsActive = isActive;
        MarkUpdated(updatedBy, DateTimeOffset.UtcNow);
    }

    public void RegisterLogin(Guid updatedBy)
    {
        LastLoginAt = DateTimeOffset.UtcNow;
        MarkUpdated(updatedBy, LastLoginAt.Value);
    }
}

public class Department : TenantSoftDeleteEntity<Guid>
{
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsSystem { get; private set; }

    private Department()
    {
    }

    public static Department Create(Guid organizationId, string code, string name, string? description, bool isSystem, Guid createdBy)
    {
        var entity = new Department
        {
            Code = NormalizeCode(code),
            Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Department name is required.", nameof(name)) : name.Trim(),
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            IsSystem = isSystem
        };

        entity.SetIdentity(Guid.NewGuid());
        entity.SetOrganization(organizationId);
        entity.MarkCreated(createdBy, DateTimeOffset.UtcNow);
        return entity;
    }

    public void Update(string code, string name, string? description, Guid updatedBy)
    {
        Code = NormalizeCode(code);
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Department name is required.", nameof(name)) : name.Trim();
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        MarkUpdated(updatedBy, DateTimeOffset.UtcNow);
    }

    private static string NormalizeCode(string code)
    {
        return string.IsNullOrWhiteSpace(code)
            ? throw new ArgumentException("Department code is required.", nameof(code))
            : code.Trim().ToUpperInvariant();
    }
}

public class Role : TenantSoftDeleteEntity<Guid>
{
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsSystem { get; private set; }

    private Role()
    {
    }

    public static Role Create(Guid organizationId, string code, string name, string? description, bool isSystem, Guid createdBy)
    {
        var entity = new Role
        {
            Code = NormalizeCode(code),
            Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Role name is required.", nameof(name)) : name.Trim(),
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            IsSystem = isSystem
        };

        entity.SetIdentity(Guid.NewGuid());
        entity.SetOrganization(organizationId);
        entity.MarkCreated(createdBy, DateTimeOffset.UtcNow);
        return entity;
    }

    public void Update(string code, string name, string? description, Guid updatedBy)
    {
        Code = NormalizeCode(code);
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Role name is required.", nameof(name)) : name.Trim();
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        MarkUpdated(updatedBy, DateTimeOffset.UtcNow);
    }

    private static string NormalizeCode(string code)
    {
        return string.IsNullOrWhiteSpace(code)
            ? throw new ArgumentException("Role code is required.", nameof(code))
            : code.Trim().ToUpperInvariant();
    }
}

public class OrganizationMembershipRole : TenantEntity<Guid>
{
    public Guid MembershipId { get; private set; }
    public OrganizationMembership Membership { get; private set; } = null!;
    public Guid RoleId { get; private set; }
    public Role Role { get; private set; } = null!;

    private OrganizationMembershipRole()
    {
    }

    public static OrganizationMembershipRole Create(Guid organizationId, Guid membershipId, Guid roleId, Guid createdBy)
    {
        var entity = new OrganizationMembershipRole
        {
            MembershipId = membershipId,
            RoleId = roleId
        };

        entity.SetIdentity(Guid.NewGuid());
        entity.SetOrganization(organizationId);
        entity.MarkCreated(createdBy, DateTimeOffset.UtcNow);
        return entity;
    }
}

public class OrganizationMembershipDepartment : TenantEntity<Guid>
{
    public Guid MembershipId { get; private set; }
    public OrganizationMembership Membership { get; private set; } = null!;
    public Guid DepartmentId { get; private set; }
    public Department Department { get; private set; } = null!;

    private OrganizationMembershipDepartment()
    {
    }

    public static OrganizationMembershipDepartment Create(Guid organizationId, Guid membershipId, Guid departmentId, Guid createdBy)
    {
        var entity = new OrganizationMembershipDepartment
        {
            MembershipId = membershipId,
            DepartmentId = departmentId
        };

        entity.SetIdentity(Guid.NewGuid());
        entity.SetOrganization(organizationId);
        entity.MarkCreated(createdBy, DateTimeOffset.UtcNow);
        return entity;
    }
}
