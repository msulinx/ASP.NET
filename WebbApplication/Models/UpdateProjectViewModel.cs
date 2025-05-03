
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebbApplication.Models;

public class UpdateProjectViewModel
{
    public string Id { get; set; } = null!;

    public IFormFile? ProjectImage { get; set; }

    public string? ImageUrl { get; set; }
    public string ProjectName { get; set; } = null!;
    
    public int ClientId { get; set; }

    public IEnumerable<SelectListItem> Clients { get; set; } = [];

    [Display(Name = "Description", Prompt = "Write a description")]
    public string RichTextContent { get; set; } = null!;

    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }
    public List<string> SelectedUserIds { get; set; } = new();

    public List<MemberViewModel> Members { get; set; } = [];
    
    public decimal Budget { get; set; }
    public int StatusId { get; set; }

    public IEnumerable<SelectListItem> Statuses { get; set; } = [];
}