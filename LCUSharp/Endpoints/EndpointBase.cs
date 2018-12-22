using LCUSharp.Http;

namespace LCUSharp.Endpoints
{
    /// <summary>
    /// An endpoint within the league client's API.
    /// </summary>
    public abstract class EndpointBase
    {
        /// <summary>
        /// The request handler.
        /// </summary>
        protected LeagueRequestHandler RequestHandler { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EndpointBase"/> class.
        /// </summary>
        /// <param name="requestHandler">The request handler.</param>
        public EndpointBase(LeagueRequestHandler requestHandler)
        {
            RequestHandler = requestHandler;
        }
    }
}
