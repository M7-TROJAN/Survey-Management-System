using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SurveyBasket.Web.Authentication;

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    // JWT settings loaded from configuration (e.g., appsettings.json)
    private readonly JwtOptions _options = options.Value;

    // Reusable token handler for generating and validating tokens
    private readonly JwtSecurityTokenHandler _tokenHandler = new();

    // Secret key encoded as byte array (used for signing and validating tokens)
    private readonly byte[] _keyBytes = Encoding.UTF8.GetBytes(options.Value.Key!);

    public (string token, int expiresIn) GenerateToken(ApplicationUser user)
    {
        // Create an array of claims representing the user identity
        var claims = GetUserClaims(user);

        // Create signing credentials using the symmetric key and HMAC-SHA256 algorithm
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(_keyBytes),
            SecurityAlgorithms.HmacSha256
        );

        // Create a JWT token with all required information
        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes),
            signingCredentials: credentials
        );

        // Serialize the token and return it with its expiry (in seconds)
        var jwt = _tokenHandler.WriteToken(token);
        return (jwt, _options.ExpiryMinutes * 60);
    }

    public string? ValidateToken(string token)
    {
        try
        {
            // Validate the token using the configured validation parameters
            _tokenHandler.ValidateToken(token, GetValidationParameters(validateLifetime: false), out SecurityToken validatedToken);

            if (validatedToken is not JwtSecurityToken jwtToken)
                return null;

            return jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

        }
        catch
        {
            return null;
        }
    }

    // Creates an array of claims from the user object
    private static Claim[] GetUserClaims(ApplicationUser user)
    {
        return
        [
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];
    }

    // Returns the parameters used to validate an incoming JWT
    private TokenValidationParameters GetValidationParameters(bool validateLifetime = true)
    {
        return new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(_keyBytes),  // Same key used to sign the token (the key from options in appsettings.json)

            ValidateIssuer = false,   // You can enable if you want to restrict the issuer
            ValidateAudience = false, // You can enable if you want to restrict the audience

            ValidateLifetime = validateLifetime,     // Check for token expiration
            ClockSkew = TimeSpan.Zero    // No clock skew allowed (strict expiry)
        };
    }
}


/*
// old code

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SurveyBasket.Web.Authentication;

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    private readonly JwtOptions _options = options.Value;

    public (string token, int expiresIn) GenerateToken(ApplicationUser user)
    {
        // first: we need to prepare the claims that will be included in the token
        Claim[] claims = [
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];

        // second: we need to prepare the symmetric security key
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key!));

        // third: we need to create the signing credentials using the symmetric key and the desired algorithm
        var singingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        // finally: we can create the JWT token using the claims, issuer, audience, expiration time, and signing credentials
        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes),
            signingCredentials: singingCredentials
        );

        // return the token as a string and the expiration time in seconds
        return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresIn: _options.ExpiryMinutes * 60);
    }

    public string? ValidateToken(string token)
    {
        // first: we need to prepare the symmetric security key (the same key used to sign the token)  
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));

        // then: we need to create a token handler to validate the token
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            // Validate the token using the symmetric key and validation parameters 
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                IssuerSigningKey = symmetricSecurityKey,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero // Disable clock skew for immediate validation
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            return jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
        }
        catch
        {
            return null;
        }
    }
}
*/