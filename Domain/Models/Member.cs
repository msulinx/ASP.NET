namespace Domain.Models;

public class Member
{
    public string Id { get; set; } = null!;
    
    public string? ImageUrl { get; set; }
    
    public string FirstName { get; set; } = null!;
    
    public string LastName { get; set; } = null!;
    
    public string Email { get; set; } = null!;
    
    public string PhoneNumber { get; set; } = null!;
    
    public string JobTitle { get; set; } = null!;
}