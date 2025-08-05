using Microsoft.AspNetCore.Identity;

namespace SurveyBasket.Web.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService, UserManager<ApplicationUser> userManager) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    [HttpPost("")]
    public async Task<IActionResult> LoginAsync(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

        return authResult switch
        {
            null => BadRequest("Invalid email or password."),
            _ => Ok(authResult)
        };
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync(
        [FromBody] RefreshTokenRequest request,
        CancellationToken cancellationToken = default)
    {
        var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
        return authResult switch
        {
            null => BadRequest("Invalid token or refresh token."),
            _ => Ok(authResult)
        };
    }

    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshTokenAsync(
        [FromBody] RefreshTokenRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
        return result ? Ok("Refresh token revoked successfully.") : BadRequest("Failed to revoke refresh token.");
    }
}