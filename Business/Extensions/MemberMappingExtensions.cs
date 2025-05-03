using Data.Entities;
using Domain.Extensions;
using Domain.FormData;
using Domain.Models;

namespace Business.Extensions;

public static class MemberMappingExtensions
{
    
    // AddFormData --> UserEntity

    public static void MapToEntity(this AddMemberFormData form, UserEntity entity)
    {
        if (form == null || entity == null)
            return;
        
        entity.FirstName = form.FirstName;
        entity.LastName = form.LastName;
        entity.Email = form.Email;
        entity.UserName = form.Email;
        entity.PhoneNumber = form.PhoneNumber;
        entity.UserImage = form.ImageUrl;

        entity.Address = new UserAddressEntity
        {
            StreetName = form.Address.StreetName,
            PostalCode = form.Address.PostalCode,
            City = form.Address.City,
        };
        
        entity.DateOfBirth = new DateTime(form.Year, form.Month, form.Day);
    }
    
    // UserEntity --> Member
    public static Member MapToMember(this UserEntity entity)
    {
        return new Member
        {
            Id = entity.Id,
            FirstName = entity.FirstName!,
            LastName = entity.LastName!,
            Email = entity.Email!,
            PhoneNumber = entity.PhoneNumber!,
            JobTitle = entity.JobTitle!,
            ImageUrl = entity.UserImage
        };
    }
    
    // UpdateFormData --> UserEntity
    public static void UpdateEntity(this UpdateMemberFormData form, UserEntity entity)
    {
        if (form == null || entity == null)
            return;
                
        entity.FirstName = form.FirstName;
        entity.LastName = form.LastName;
        entity.Email = form.Email;
        entity.JobTitle = form.JobTitle;
        entity.PhoneNumber = form.PhoneNumber;
        entity.UserImage = form.ImageUrl;

        if (form.Address != null)
            entity.Address = form.Address.MapTo<UserAddressEntity>();
    }

}