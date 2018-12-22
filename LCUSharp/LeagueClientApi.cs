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

        private readonly LeagueProcessHandler _processHandler;
        private readonly LockFileHandler _lockFileHandler;

        /// <inheritdoc />
        public LeagueRequestHandler RequestHandler { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LeagueClientApi"/> class.
        /// </summary>
        public LeagueClientApi()
        {
            _lockFileHandler = new LockFileHandler();
            _processHandler = new LeagueProcessHandler();
            _processHandler.Exited += OnProcessExited;
        }

        /// <inheritdoc />
        public async Task ConnectAsync()
        {
            await _processHandler.WaitForProcessAsync().ConfigureAwait(false);
            var (port, token) = await _lockFileHandler.ParseLockFileAsync(_processHandler.BasePath).ConfigureAwait(false);

            InitRequestHandlerAndEndpoints(port, token);
        }

        /// <summary>
        /// Initializes the <see cref="LeagueRequestHandler"/> and endpoints.
        /// </summary>
        /// <param name="port">The league client API's port.</param>
        /// <param name="token">The authentication token.</param>
        private void InitRequestHandlerAndEndpoints(int port, string token)
        {
            RequestHandler = new LeagueRequestHandler(port, token);
        }

        /// <summary>
        /// Called when the league client is exited.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnProcessExited(object sender, EventArgs e)
        {
            Disconnected(sender, e);
        }
    }
}
