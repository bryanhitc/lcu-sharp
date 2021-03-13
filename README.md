# lcu-sharp

An API wrapper for the League of Legends client.

For the LCU API documentation, check out [Rift Explorer.](https://github.com/Pupix/rift-explorer)

## Usage

### Request example

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

![Usage Request Run](https://i.imgur.com/OCRPHes.gif)

### Event example

```cs
public event EventHandler<LeagueEvent> GameFlowChanged;
private readonly TaskCompletionSource<bool> _work = new TaskCompletionSource<bool>(false);

public async Task EventExampleAsync()
{
    // Initialize a connection to the league client.
    var api = await LeagueClientApi.ConnectAsync();
    Console.WriteLine("Connected!");

    // Register game flow event.
    GameFlowChanged += OnGameFlowChanged;
    api.EventHandler.Subscribe("/lol-gameflow/v1/gameflow-phase", GameFlowChanged);

    // Wait until work is complete.
    await _work.Task;
    Console.WriteLine("Done.");
}

private void OnGameFlowChanged(object sender, LeagueEvent e)
{
    var result = e.Data.ToString();
    var state = string.Empty;

    switch (result)
    {
        case "None":
            state = "main menu";
            break;
        case "Lobby":
            state = "lobby";
            break;
        case "ChampSelect":
            state = "champ select";
            break;
        case "GameStart":
            state = "game started";
            break;
        case "InProgress":
            state = "game";
            break;
        case "WaitingForStats":
            state = "waiting for stats";
            break;
        default:
            state = $"unknown state: {result}";
            break;
    }

    // Print new state and set work to complete.
    Console.WriteLine($"Status update: Entered {state}.");
    _work.SetResult(true);
}
```

![Usage Event Run](https://i.imgur.com/mavltPs.gif)

## License

Copyright (c) 2020 Bryan Hitchcock. All rights reserved.

Licensed under the [MIT](./LICENSE.txt) license.
