namespace WebbApplication.Models;

public class MemberViewModel
{
    public string Id { get; set; } = null!;
    
    public string? ImageUrl { get; set; }
    
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }

    public string? JobTitle { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }
    
    public int PageSize { get; set; }
}