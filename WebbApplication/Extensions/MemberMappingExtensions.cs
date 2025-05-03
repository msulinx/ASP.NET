
using Domain.Models;
using WebbApplication.Models;

namespace WebbApplication.Extensions;

public static class MemberMappingExtensions
{
    // Enskild member --> Viewmodel
    public static MemberViewModel MapToViewModel(this Member member, int pageSize)
    {
        return new MemberViewModel
        {
            Id = member.Id,
            FirstName = member.FirstName,
            LastName = member.LastName,
            Email = member.Email,
            PhoneNumber = member.PhoneNumber,
            JobTitle = member.JobTitle,
            ImageUrl = member.ImageUrl,
            PageSize = pageSize
        };
    }

    /* Lista av members --> lista av viewmodel */ 
    public static List<MemberViewModel> MapToViewModels(this IEnumerable<Member> members, int pageSize)
    {
        return members.Select(m => m.MapToViewModel(pageSize)).ToList();
    }
}