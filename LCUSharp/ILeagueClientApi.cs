using LCUSharp.Http;
using System.Threading.Tasks;

namespace LCUSharp
{
    /// <summary>
    /// Represents an interface that can directly communicate to the league client's API.
    /// </summary>
    public interface ILeagueClientApi
    {
        /// <summary>
        /// The request handler.
        /// </summary>
        LeagueRequestHandler RequestHandler { get; }

        /// <summary>
        /// Connects to the league client's API.
        /// </summary>
        /// <returns></returns>
        Task ConnectAsync();
    }
}
