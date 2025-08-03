using Microsoft.AspNetCore.Identity;
using SurveyBasket.Web.Authentication;
using System.Security.Cryptography;

namespace SurveyBasket.Web.Services;

public class AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly int _RefreshTokenExpiryDays = 30;

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

        // Generate a refresh token
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_RefreshTokenExpiryDays);

        // Store the refresh token in the user entity (this will save it to the database automatically)
        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpiresOn = DateTime.UtcNow.AddDays(_RefreshTokenExpiryDays),
        });

        // Save changes to the user entity
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            // Handle the error, e.g., log it or throw an exception
            return null;
        }

        // simulate a token response  
        return new AuthResponse(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            token,
            expiresIn,
            refreshToken,
            refreshTokenExpiration
        );
    }

    private static string GenerateRefreshToken()
    {
        // Generate a random 64-byte array and convert it to a Base64 string
        var refreshTokenBytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(refreshTokenBytes);
    }
}
