namespace SurveyBasket.Web.Services;

public interface IPollService
{
    IEnumerable<Poll> GetAll();
    Poll? Get(int id);

    Poll Add(Poll pool);

    bool Update(int id, Poll pool);

    bool Delete(int id);
}
