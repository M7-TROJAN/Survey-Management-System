namespace SurveyBasket.Web.Contracts.Authentication;

public record LoginRequest(
    string Email,
    string Password
);