namespace Domain.FormData;

public class UpdateProjectFormData
{
    public string Id { get; set; } = null!;
    
    public string? ImageUrl { get; set; }
    public string ProjectName { get; set; } = null!;
    
    public int ClientId { get; set; }
    public List<string> SelectedUserIds { get; set; } = [];

    public string? Description { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public decimal Budget { get; set; }
    
    public int StatusId { get; set; }
}