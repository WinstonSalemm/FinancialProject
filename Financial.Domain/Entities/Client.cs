public enum ClientType
{
    Individual,
    Company
}
public class Client
{
    public Guid Id { get; set; }

    public Guid OrganizationId { get; set; }
    public ClientType Type { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}