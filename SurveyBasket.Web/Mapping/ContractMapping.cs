using SurveyBasket.Web.Contracts.Requests;
using SurveyBasket.Web.Contracts.Responces;

namespace SurveyBasket.Web.Mapping;

public static class ContractMapping
{
    public static PoolResponce MapToResponse(this Poll poll)
    {
        return new PoolResponce
        {
            Id = poll.Id,
            Title = poll.Title,
            Description = poll.Description
        };
    }

    public static IEnumerable<PoolResponce> MapToResponse(this IEnumerable<Poll> polls)
    {
        return polls.Select(p => p.MapToResponse());
    }

    public static Poll MapToPoll(this CreatePollRequest PollRequest)
    {
        return new Poll
        {
            Title = PollRequest.Title,
            Description = PollRequest.Description
        };
    }
}