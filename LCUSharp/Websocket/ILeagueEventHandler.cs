using System;

namespace LCUSharp.Websocket
{
    /// <summary>
    /// Handles operations relating to capturing and processing league events via WebSockets.
    /// </summary>
    public interface ILeagueEventHandler
    {
        /// <summary>
        /// EventHandler used for the Websocket's received messages.
        /// </summary>
        EventHandler<LeagueEvent> MessageReceived { get; set; }

        /// <summary>
        /// EventHandler used for the Websocket's error messages.
        /// </summary>
        EventHandler<string> ErrorReceived { get; set; }

        /// <summary>
        /// Subscribes the event handler to the specified event uri.
        /// </summary>
        /// <param name="uri">The league client event uri.</param>
        /// <param name="eventHandler">The event handler.</param>
        void Subscribe(string uri, EventHandler<LeagueEvent> eventHandler);

        /// <summary>
        /// Unsubscribes the event handlers subscribed to the specified event uri.
        /// </summary>
        /// <param name="uri">The uri to unsubscribe from.</param>
        void Unsubscribe(string uri);

        /// <summary>
        /// Unsubcribes all event handlers.
        /// </summary>
        void UnsubscribeAll();
    }
}
