
using SurveyBasket.Web.Models;

namespace SurveyBasket.Web.Services;

public class PollService : IPollService
{
    public IEnumerable<Poll> GetAll() =>
    _polls;

    public Poll? Get(int id) =>
        _polls.FirstOrDefault(p => p.Id == id);

    public Poll Add(Poll pool)
    {
        pool.Id = _polls.Count + 1; // Simulate auto-increment ID
        _polls.Add(pool);
        return pool;
    }

    public bool Update(int id, Poll pool)
    {
        var currentPoll = Get(id);

        if (currentPoll is null)
            return false;

        currentPoll.Title = pool.Title;
        currentPoll.Description = pool.Description;

        return true;
    }

    public bool Delete(int id)
    {
        var pool = Get(id);

        if (pool is null)
            return false;

        _polls.Remove(pool);

        return true;
    }


    // This method simulates a database or external service call to retrieve polls.
    private static readonly List<Poll> _polls =
    [
            new Poll { Id = 1, Title = "Favorite Color", Description = "What is your favorite color?" },
            new Poll { Id = 2, Title = "Best Programming Language", Description = "Which programming language do you prefer?" },
            new Poll { Id = 3, Title = "Best Framework", Description = "Which framework do you prefer?" },
            new Poll { Id = 4, Title = "Best Database", Description = "Which database do you prefer?" },
            new Poll { Id = 5, Title = "Best Cloud Provider", Description = "Which cloud provider do you prefer?" },
            new Poll { Id = 6, Title = "Best IDE", Description = "Which IDE do you prefer?" },
            new Poll { Id = 7, Title = "Best Operating System", Description = "Which operating system do you prefer?" },
            new Poll { Id = 8, Title = "Best Browser", Description = "Which browser do you prefer?" },
            new Poll { Id = 9, Title = "Best Mobile OS", Description = "Which mobile operating system do you prefer?" },
            new Poll { Id = 10, Title = "Best Game", Description = "Which game do you prefer?" },
            new Poll { Id = 11, Title = "Best Movie", Description = "Which movie do you prefer?" },
            new Poll { Id = 12, Title = "Best TV Show", Description = "Which TV show do you prefer?" },
            new Poll { Id = 13, Title = "Best Book", Description = "Which book do you prefer?" },
            new Poll { Id = 14, Title = "Best Music Genre", Description = "Which music genre do you prefer?" },
            new Poll { Id = 15, Title = "Best Sport", Description = "Which sport do you prefer?" },
            new Poll { Id = 16, Title = "Best Hobby", Description = "Which hobby do you prefer?" },
            new Poll { Id = 17, Title = "Best Food", Description = "Which food do you prefer?" },
            new Poll { Id = 18, Title = "Best Drink", Description = "Which drink do you prefer?" },
            new Poll { Id = 19, Title = "Best Vacation Destination", Description = "Which vacation destination do you prefer?" },
            new Poll { Id = 20, Title = "Best Pet", Description = "Which pet do you prefer?" }
    ];
}
