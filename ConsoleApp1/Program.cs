using LCUSharp;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Initialize a connection to the league client.
            var api = await LeagueClientApi.ConnectAsync();

            // Update the current summoner's profile icon to 23.
            var body = new { profileIconId = 23 };
            var json = await api.RequestHandler.GetJsonResponseAsync(HttpMethod.Put, "lol-summoner/v1/current-summoner/icon", body);
        }
    }
}
