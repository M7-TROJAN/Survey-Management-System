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

    public async Task<AuthResponse?> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        // Validate the JWT token
        var userId = _jwtProvider.ValidateToken(token);

        if (userId is null)
            return null;

        // Find the user by ID
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return null;

        // Check if the provided refresh token is valid
        var userRefreshToken = user.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshToken && rt.IsActive);

        if (userRefreshToken is null)
            return null;

        // Generate a new JWT token
        var (newToken, expiresIn) = _jwtProvider.GenerateToken(user);

        // Generate a new refresh token
        var newRefreshToken = GenerateRefreshToken();
        var newRefreshTokenExpiration = DateTime.UtcNow.AddDays(_RefreshTokenExpiryDays);

        // Mark the old refresh token as revoked (because the user can use it only once)
        userRefreshToken.RevokedOn = DateTime.UtcNow;

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresOn = newRefreshTokenExpiration,
        });

        // Save changes to the user entity
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            // Handle the error, e.g., log it or throw an exception
            return null;
        }

        // Return the new token response
        return new AuthResponse(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            newToken,
            expiresIn,
            newRefreshToken,
            newRefreshTokenExpiration
        );
    }

    public async Task<bool> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        // Validate the JWT token
        var userId = _jwtProvider.ValidateToken(token);

        if (userId is null)
            return false;

        // Find the user by ID
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return false;

        // Find the refresh token to revoke
        var userRefreshToken = user.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshToken && rt.IsActive);

        if (userRefreshToken is null)
            return false;

        // Mark the refresh token as revoked
        userRefreshToken.RevokedOn = DateTime.UtcNow;

        // Save changes to the user entity
        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded;
    }

    private static string GenerateRefreshToken()
    {
        // Generate a random 64-byte array and convert it to a Base64 string
        var refreshTokenBytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(refreshTokenBytes);
    }
}