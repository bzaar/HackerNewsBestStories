using System.Collections.Concurrent;
using System.Text.Json;

namespace HackerNewsBestStories;

/// <summary>
/// Caches stories returned by the /beststories and /item endpoints
/// to reduce the load on the Hacker News API.
/// </summary>
public class BestStoriesCache
{
    private readonly HttpClient _httpClient;
    private readonly Lazy<Task<int[]>> _bestStories;
    private readonly ConcurrentDictionary<int, Task<Story>> _stories = new();

    public BestStoriesCache(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _bestStories = new Lazy<Task<int[]>>(GetAllBestStories);
    }
    
    public async Task<Story[]> GetTop(int n)
    {
        int[] ids = await _bestStories.Value;
        var tasks = ids.Take(n).Select(GetStoryTask).ToArray();
        Story[] stories = await Task.WhenAll(tasks);
        return stories;
    }

    private async Task<int[]> GetAllBestStories()
    {
        return await Fetch<int[]>("/beststories");
    }

    private Task<Story> GetStoryTask(int id)
    {
        return _stories.GetOrAdd(id, FetchStory);
    }

    private async Task<Story> FetchStory(int id)
    {
        var story = await Fetch<Item>("/item/"+id);
        return new Story(
            Title: story.Title,
            Url: story.Url,
            PostedBy: story.By,
            Time: UnixTimeToDateTime(story.Time),
            Score: story.Score,
            CommentCount: story.Descendants);
    }

    private async Task<T> Fetch<T>(string url)
    {
        var responseMessage = await _httpClient.GetAsync($"/v0{url}.json");
        responseMessage.EnsureSuccessStatusCode();
        var stream = await responseMessage.Content.ReadAsStreamAsync();
        return await JsonSerializer.DeserializeAsync<T>(stream, JsonSerializerOptions)
               ?? throw new Exception("JSON deserialisation failed.");
    }

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private static DateTime UnixTimeToDateTime(int secondsSince1970)
    {
        return new DateTime(1970, 1, 1) + TimeSpan.FromSeconds(secondsSince1970);
    }
}