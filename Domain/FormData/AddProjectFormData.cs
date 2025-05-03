namespace Domain.FormData;

public class AddProjectFormData
{
    public string? ImageUrl { get; set; }
    
    public string ProjectName { get; set; } = null!;
    
    public int SelectedClientId { get; set; }
    
    public string? Description { get; set; }

    public DateTime? StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public decimal Budget { get; set; }
    
    public List<string> SelectedUserIds { get; set; } = [];
}