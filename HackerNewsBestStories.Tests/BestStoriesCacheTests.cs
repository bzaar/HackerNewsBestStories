namespace HackerNewsBestStories.Tests;

public class BestStoriesCacheTests
{
    [Fact]
    public async Task GetZeroTopStories_ZeroStoriesReturned()
    {
        BestStoriesCache cache = SetupCache(new MockHttpHandler());

        var stories = await cache.GetTop(0);
        
        Assert.Empty(stories);
    }

    [Fact]
    public async Task GetTopStoriesOnce()
    {
        var mockHttpHandler = new MockHttpHandler();
        var cache = SetupCache(mockHttpHandler);
        
        var stories = await cache.GetTop(5);
        
        Assert.Equal(3, stories.Length); // the mock returns fewer stories than requested
        Assert.Equal("Story 1", stories[0].Title);
        Assert.Equal("Story 2", stories[1].Title);
        Assert.Equal("Story 3", stories[2].Title);
        Assert.Equal(4, mockHttpHandler.CallCount);
    }

    [Fact]
    public async Task GetTopStoriesTwice_UnderlyingServiceCalledOnce()
    {
        var mockHttpHandler = new MockHttpHandler();
        var cache = SetupCache(mockHttpHandler);
        
        var stories1 = await cache.GetTop(5);
        Assert.Equal(4, mockHttpHandler.CallCount);
        var stories2 = await cache.GetTop(5);
        Assert.Equal(4, mockHttpHandler.CallCount); // no more calls should be done
        
        Assert.Equal(3, stories1.Length); // the mock returns fewer stories than requested
        Assert.Equal("Story 1", stories1[0].Title);
        Assert.Equal("Story 2", stories1[1].Title);
        Assert.Equal("Story 3", stories1[2].Title);
        Assert.Equal("Story 1", stories2[0].Title);
        Assert.Equal("Story 2", stories2[1].Title);
        Assert.Equal("Story 3", stories2[2].Title);
    }

    private static BestStoriesCache SetupCache(HttpMessageHandler httpMessageHandler)
    {
        var httpClient = new HttpClient(httpMessageHandler);
        httpClient.BaseAddress = new Uri("https://test.com");
        var cache = new BestStoriesCache(httpClient);
        return cache;
    }
}