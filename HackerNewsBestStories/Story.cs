namespace HackerNewsBestStories;

public record Story(string Title, string Url, string PostedBy, DateTime Time, int Score, int CommentCount);

public record HNStory(string Title, string Url, string By, int Time, int Score, int Descendants);

