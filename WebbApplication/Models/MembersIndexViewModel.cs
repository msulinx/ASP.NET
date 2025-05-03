namespace WebbApplication.Models;

public class MembersIndexViewModel
{
    public List<MemberViewModel> Members { get; set; } = new();

    public AddMemberViewModel AddMemberFormData { get; set; } = new();

    public UpdateMemberViewModel UpdateMemberFormData { get; set; } = new();
    
    public List<UpdateMemberViewModel> UpdateMemberViewModels { get; set; } = new();
    
    public int CurrentPage { get; set; } 
    public int PageSize { get; set; }

    public List<int> PageSizeOptions { get; set; } = new() { 2, 4, 6 };
    public int TotalPages { get; set; }
}