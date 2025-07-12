using Microsoft.AspNetCore.Identity;

namespace SurveyBasket.Web.Entities;

public sealed class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    // navigation properties
    public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}