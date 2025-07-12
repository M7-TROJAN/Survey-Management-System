
using Microsoft.AspNetCore.Identity;
using SurveyBasket.Web.Authentication;

namespace SurveyBasket.Web.Services;

public class AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    public async Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        // check if you have a user with this email
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return null;

        // check password is correct
        var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
        if (!isPasswordValid)
            return null;

        //generate jwt token
        var (token, expiresIn) = _jwtProvider.GenerateToken(user);


        // simulate a token response
        return new AuthResponse(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            token,
            expiresIn
        );
    }
}
