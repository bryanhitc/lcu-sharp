using LCUSharp.Endpoints.ProcessControl;
using LCUSharp.Endpoints.RiotClient;
using LCUSharp.Http;
using LCUSharp.Utility;
using System;
using System.Threading.Tasks;

namespace LCUSharp
{
    /// <inheritdoc />
    public class LeagueClientApi : ILeagueClientApi
    {
        public event EventHandler Disconnected;

        /// <inheritdoc />
        public LeagueRequestHandler RequestHandler { get; }

        /// <inheritdoc />
        public IRiotClientEndpoint RiotClientEndpoint { get; }

        /// <inheritdoc />
        public IProcessControlEndpoint ProcessControlEndpoint { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LeagueClientApi"/> class.
        /// </summary>
        /// <param name="port">The league client API's port.</param>
        /// <param name="token">The authentication token.</param>
        private LeagueClientApi(int port, string token)
        {
            RequestHandler = new LeagueRequestHandler(port, token);
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

            return new LeagueClientApi(port, token);
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
