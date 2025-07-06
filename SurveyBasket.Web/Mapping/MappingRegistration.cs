namespace SurveyBasket.Web.Mapping;

public class MappingRegistration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Map CreatePollRequest to Poll
        config.NewConfig<CreatePollRequest, Poll>()
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Description, src => src.Description);

        // Map Poll to PollResponse
        config.NewConfig<Poll, PollResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Description, src => src.Description);
    }
}
