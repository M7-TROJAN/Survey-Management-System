namespace SurveyBasket.Web.Contracts.Validations;

public class CreatePollRequestValidator : AbstractValidator<PollRequest>
{
    public CreatePollRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .Length(3, 100);

        RuleFor(x => x.Summary)
            .NotEmpty()
            .Length(3, 1000);
    }
}