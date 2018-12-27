using LCUSharp.Http;
using LCUSharp.Http.Endpoints;
using LCUSharp.Utility;
using LCUSharp.Websocket;
using System;
using System.Threading.Tasks;

namespace LCUSharp
{
    /// <inheritdoc />
    public class LeagueClientApi : ILeagueClientApi
    {
        public event EventHandler Disconnected;

        /// <inheritdoc />
        public ILeagueRequestHandler RequestHandler { get; }

        /// <inheritdoc />
        public ILeagueEventHandler EventHandler { get; }

        /// <inheritdoc />
        public IRiotClientEndpoint RiotClientEndpoint { get; }

        /// <inheritdoc />
        public IProcessControlEndpoint ProcessControlEndpoint { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LeagueClientApi"/> class.
        /// </summary>
        /// <param name="port">The league client API's port.</param>
        /// <param name="token">The authentication token.</param>
        private LeagueClientApi(int port, string token, ILeagueRequestHandler requestHandler, ILeagueEventHandler eventHandler)
        {
            RequestHandler = requestHandler;
            EventHandler = eventHandler;
            RiotClientEndpoint = new RiotClientEndpoint(RequestHandler);
            ProcessControlEndpoint = new ProcessControlEndpoint(RequestHandler);
        }

        /// <inheritdoc />
        public static async Task<LeagueClientApi> ConnectAsync()
        {
            var processHandler = new LeagueProcessHandler();
            var lockFileHandler = new LockFileHandler();

            await processHandler.WaitForProcessAsync().ConfigureAwait(false);
            var (port, token) = await lockFileHandler.ParseLockFileAsync(processHandler.BasePath).ConfigureAwait(false);

            var requestHandler = new LeagueRequestHandler(port, token);
            var eventHandler = new LeagueEventHandler(port, token);
            await eventHandler.ConnectAsync().ConfigureAwait(false);

            return new LeagueClientApi(port, token, requestHandler, eventHandler);
        }

        /// <summary>
        /// Called when the league client is exited.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnProcessExited(object sender, EventArgs e)
        {
            Disconnected?.Invoke(sender, e);
        }
    }
}
