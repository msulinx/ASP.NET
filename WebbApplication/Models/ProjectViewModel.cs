namespace WebbApplication.Models;

public class ProjectViewModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public string? ImageUrl { get; set; }
    
    public string ProjectName { get; set; } = null!;
    
    public string ClientName { get; set; } = null!;
    
    public string Description { get; set; } = null!;
    
    public DateTime? StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public string Status { get; set; } = null!;

    public List<MemberViewModel> Members { get; set; } = new();
    public DateTime Created { get; set; }
    
    public int PageSize { get; set; }
    
    /* All kod som rör detta är genererad av Chat GPT för att ändra färg på projektstatus
    till röd när det är mindre än 1 vecka kvar till slutdatum */
    public bool IsDueSoon =>
        EndDate.HasValue &&
        (EndDate.Value - DateTime.Now).TotalDays < 7 &&
        (EndDate.Value - DateTime.Now).TotalDays >= 0;
}