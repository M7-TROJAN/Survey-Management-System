namespace SurveyBasket.Web.Contracts.Requests;

public record CreatePollRequest(
    string Title,
    string Description
);