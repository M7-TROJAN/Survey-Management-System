namespace SurveyBasket.Web.Contracts.Authentication;

public record RefreshTokenRequest
(
    string Token,
    string RefreshToken
);
