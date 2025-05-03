namespace Domain.Models;

public class Project
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public string ProjectName { get; set; } = null!;
    
    public string? ImageUrl { get; set; }
    
    public string? Description { get; set; }
    
    public string ClientName { get; set; } = null!;
    
    public int ClientId { get; set; }
    
    public DateTime? StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public decimal? Budget { get; set; }
    
    public List<Member> Members { get; set; } = new();
    
    public string Status { get; set; } = null!;
    
    public DateTime Created { get; set; }
}