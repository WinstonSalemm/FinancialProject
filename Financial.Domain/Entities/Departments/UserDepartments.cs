public class UserDepartment
{
    public Guid UserId { get; set; }
    public Guid DepartmentId { get; set; }
    public User User { get; set; } = null!;
    public Department Department { get; set; } = null!;
}