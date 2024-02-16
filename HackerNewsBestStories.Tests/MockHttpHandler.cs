using System.Net;

namespace HackerNewsBestStories.Tests;

class MockHttpHandler : HttpClientHandler
{
    public int CallCount;
    
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Interlocked.Increment(ref CallCount);
        
        var path = request.RequestUri?.ToString();
        
        string body = path switch
        {
            "https://test.com/v0/beststories.json" => "[1,2,3]",
            "https://test.com/v0/item/1.json" => "{'title': 'Story 1'}",
            "https://test.com/v0/item/2.json" => "{'title': 'Story 2'}",
            "https://test.com/v0/item/3.json" => "{'title': 'Story 3'}",
            _ => throw new Exception("Not set up to handle " + path)
        };

        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(body.Replace("'", "\""))
        };
        
        return Task.FromResult(response);
    }
}