# Best Hacker News Stories API

This Web API returns best Hacker News stories details fetched from the [Hacker News API](https://github.com/HackerNews/API).

It exposes the /BestStories?n=NNN endpoint where NNN is the desired number of stories to return.

E.g. /BestStories?n=3 might return:

```json
[
  {
    "title": "Sora: Creating video from text",
    "url": "https://openai.com/sora",
    "postedBy": "davidbarker",
    "time": "2024-02-15T18:14:18",
    "score": 1766,
    "commentCount": 1066
  },
  {
    "title": "European Court of Human Rights bans weakening of secure end-to-end encryption",
    "url": "https://www.eureporter.co/world/human-rights-category/european-court-of-human-rights-echr/2024/02/14/european-court-of-human-rights-bans-weakening-of-secure-end-to-endencryption-the-end-of-eus-chat-control-csar-mass-surveillance-plans/",
    "postedBy": "robtherobber",
    "time": "2024-02-14T13:44:45",
    "score": 1603,
    "commentCount": 248
  },
  {
    "title": "Is something bugging you?",
    "url": "https://antithesis.com/blog/is_something_bugging_you/",
    "postedBy": "wwilson",
    "time": "2024-02-13T12:13:12",
    "score": 1154,
    "commentCount": 406
  }
]
```

When n is 0 or negative, an empty array is returned.

The services caches the responses from the HN API: it only calls the /beststories endpoint once and the /item endpoint once for each item/story.

## How to Run

One a machine with .NET 7 installed, cd into the HackerNewsBestStories folder and type ```dotnet run```:

```cmd
C:\Code\HackerNewsBestStories> cd HackerNewsBestStories
C:\Code\HackerNewsBestStories\HackerNewsBestStories> dotnet run
```

Navigate to http://&lt;service base url&gt;/swagger/ and try out the /BestStories endpoint.


## Assumptions

This solution assumes that the /beststories endpoint returns ids of stories ordered by their score (starting from the highest score). This seems like a fair assumption to make since the /BestStories endpoint has always returned descending scores during development.



## Limitations

The API only returns up to 500 best stories because it relies on the /beststories endpoint of the HN API which has the same limitation. Theoretically, one could implement the same API by collecting the scores for all items (using the /maxitem and /item endpoints) but that would generate lots of network traffic which is something we're trying to avoid.

It would be reasonable to assume that the best stories returned by the HN API are updated with some regularity. To reflect these changes, one could add cache invalidation. But as this is one of the two hardest problems in computer science, I decided to leave it out of this coding test.