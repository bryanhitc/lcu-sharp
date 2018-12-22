# lcu-sharp
An API wrapper for the League of Legends client.

For the LCU API documentation, check out [Rift Explorer.](https://github.com/Pupix/rift-explorer)

## Usage
```cs
// Initialize a connection to the league client.
var api = await LeagueClientApi.ConnectAsync();

// Show the client.
await api.RiotClientEndpoint.ShowUxAsync();
await Task.Delay(1000);

// Update the current summoner's profile icon to 23.
var body = new { profileIconId = 23 };
var queryParameters = Enumerable.Empty<string>();
var json = await api.RequestHandler.GetJsonResponseAsync(HttpMethod.Put, "lol-summoner/v1/current-summoner/icon",
                                                         queryParameters, body);

// Minimize the client.
await Task.Delay(1000);
await api.RiotClientEndpoint.MinimizeUxAsync();
```

![Usage Run](https://i.imgur.com/OCRPHes.gif)
