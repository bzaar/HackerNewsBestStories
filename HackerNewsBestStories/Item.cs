namespace HackerNewsBestStories;

// Disable the 'class never instantiated' warning - it is created through Reflection.
// ReSharper disable once ClassNeverInstantiated.Global

public record Item(
    string Title,
    string Url,
    string By,
    int Time,
    int Score,
    int Descendants);