using LCUSharp.Endpoints.ProcessControl;
using LCUSharp.Endpoints.RiotClient;
using LCUSharp.Http;
using LCUSharp.Websocket;

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
        /// The event handler.
        /// </summary>
        ILeagueEventHandler EventHandler { get; }

        /// <summary>
        /// The riot client endpoint.
        /// </summary>
        IRiotClientEndpoint RiotClientEndpoint { get; }

        /// <summary>
        /// The process control endpoint.
        /// </summary>
        IProcessControlEndpoint ProcessControlEndpoint { get; }
    }
}
