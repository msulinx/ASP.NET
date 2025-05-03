using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebbApplication.Models;

public class AddProjectViewModel
{
    public IFormFile? ProjectImage { get; set; }

    public string? ImageUrl { get; set; }
    
    [Required(ErrorMessage = "Project name is required.")]
    public string ProjectName { get; set; } = null!;

    [Required(ErrorMessage = "Client is required.")]
    public int SelectedClientId { get; set; }

    public IEnumerable<SelectListItem> Clients { get; set; } = [];

    [Display(Name = "Description", Prompt = "Write a description")]
    public string RichTextContent { get; set; } = null!;

    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Start date is required.")]
    public DateTime StartDate { get; set; }

    [DataType(DataType.Date)]
    [Required(ErrorMessage = "End date is required.")]
    public DateTime EndDate { get; set; }

    [Required(ErrorMessage = "Choose at least one project owner")]
    public List<string> SelectedUserIds { get; set; } = new();

    public List<MemberViewModel> Members { get; set; } = [];

    [Required(ErrorMessage = "Budget is required.")]
    public decimal Budget { get; set; }
    
}