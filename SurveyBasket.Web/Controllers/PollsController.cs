namespace SurveyBasket.Web.Controllers;

[Route("api/[controller]")] // this will map to /api/polls
[ApiController]
public class PollsController(IPollService pollService) : ControllerBase
{
    private readonly IPollService _pollService = pollService;

    [HttpGet]
    [Route("")]
    public IActionResult GetAll()
    {
        var polls = _pollService.GetAll();
        if (polls is null || !polls.Any())
        {
            return NotFound("No polls found.");
        }
        return Ok(polls);
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult Get(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid poll ID.");
        }

        var poll = _pollService.Get(id);

        return poll is null ? NotFound() : Ok(poll);
    }

    [HttpPost]
    [Route("")]
    public IActionResult Add([FromBody] Poll request)
    {
        if (request is null)
        {
            return BadRequest("Poll data is required.");
        }
        if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Description))
        {
            return BadRequest("Poll title and description are required.");
        }
        var addedPoll = _pollService.Add(request);
        return CreatedAtAction(nameof(Get), new { id = addedPoll.Id }, addedPoll);
    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult Update(int id, [FromBody] Poll request)
    {
        var result = _pollService.Update(id, request);

        if (!result)
            return NotFound("Poll not found or update failed.");

        return NoContent();

    }

    [HttpDelete]
    [Route("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _pollService.Delete(id);

        if (!result)
            return NotFound("Poll not found or update failed.");

        return NoContent();
    }
}