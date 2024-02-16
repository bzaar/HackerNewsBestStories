using Microsoft.AspNetCore.Mvc;

namespace HackerNewsBestStories;

[ApiController]
[Route("[controller]")]
public class BestStoriesController : ControllerBase
{
    private readonly BestStoriesCache _bestStoriesCache;

    public BestStoriesController(BestStoriesCache bestStoriesCache)
    {
        _bestStoriesCache = bestStoriesCache;
    }

    [HttpGet(Name = "GetBestStories")]
    public async Task<IEnumerable<Story>> Get(int n)
    {
        return await _bestStoriesCache.GetTop(n);
    }
}