using System.ComponentModel.DataAnnotations;

namespace WebbApplication.Models;

public class UpdateMemberViewModel
{
    public string Id { get; set; } = null!;
    
    [Display(Name = "Upload Image")]
    public IFormFile? MemberImage { get; set; }
    
    public string? ImageUrl { get; set; }
    
    public string FirstName { get; set; } = null!;
    
    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;
    
    public string PhoneNumber { get; set; } = null!;
    
    public string JobTitle { get; set; } = null!;

    public UserAddressViewModel Address { get; set; } = new();
    
    public int Day { get; set; }
    
    public int Month { get; set; }
    
    public int Year { get; set; }
    
    public string? Role { get; set; }
}