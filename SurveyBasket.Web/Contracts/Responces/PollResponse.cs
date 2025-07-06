namespace SurveyBasket.Web.Contracts.Responces;
public record PollResponse(
    int Id,
    string Title,
    string Description
);