// ReSharper disable NotAccessedPositionalProperty.Global
namespace HackerNewsBestStories;

public record Story(string Title, string Url, string PostedBy, DateTime Time, int Score, int CommentCount);