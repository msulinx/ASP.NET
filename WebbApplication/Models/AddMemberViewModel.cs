using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebbApplication.Models;

public class AddMemberViewModel

{
    [Display(Name = "Upload Image")]
    public IFormFile? MemberImage { get; set; }
    
    public string? ImageUrl { get; set; }
    
    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; } = null!;
    
    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; } = null!;
    
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = null!;
    
    [Required(ErrorMessage = "Phone number is required")]
    public string PhoneNumber { get; set; } = null!;

    [Required(ErrorMessage = "Job title is required.")]
    public string JobTitle { get; set; } = null!;

    public UserAddressViewModel Address { get; set; } = new();
    
    public int Day { get; set; }
    
    public int Month { get; set; }
    
    public int Year { get; set; }
    
    [Required(ErrorMessage = "Role is required")]
    public string Role { get; set; } = "Member";

    public List<SelectListItem> Roles { get; set; } = new();
}