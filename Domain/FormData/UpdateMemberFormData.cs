namespace Domain.FormData;

public class UpdateMemberFormData

{
    public string Id { get; set; } = null!;
    
    public string? ImageUrl { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? JobTitle { get; set; }

    public UserAddressFormData Address { get; set; } = new();

    public int Day { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public string? Role { get; set; }
}