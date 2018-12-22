# lcu-sharp
An API wrapper for the League of Legends client.

For the LCU API documentation, check out [Rift Explorer.](https://github.com/Pupix/rift-explorer)

## Usage
```cs
// Initialize a connection to the league client.
var api = new LeagueClientApi();
await api.ConnectAsync();

// Update the current summoner's profile icon to 23.
var body = new { profileIconId = 23 };
var json = await api.RequestHandler.GetJsonResponseAsync(HttpMethod.Put, "lol-summoner/v1/current-summoner/icon", body);
```
