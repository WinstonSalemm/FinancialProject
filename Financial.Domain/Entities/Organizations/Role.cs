public class Role
{
    public Guid Id { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public ICollection<User> Users { get; set; }
    = new List<User>();
}