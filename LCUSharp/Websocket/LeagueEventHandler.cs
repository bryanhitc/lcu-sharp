using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using WebSocketSharp;

namespace LCUSharp.Websocket
{
    /// <inheritdoc />
    internal class LeagueEventHandler : ILeagueEventHandler
    {
        private const int ClientEventData = 2;
        private const int ClientEventNumber = 8;

        /// <summary>
        /// Contains all event handlers that are subscribed to an event uri.
        /// </summary>
        private readonly Dictionary<string, List<EventHandler<LeagueEvent>>> _subscribers;

        /// <summary>
        /// Websocket used to connect to the League Client's backend.
        /// </summary>
        private WebSocket _webSocket;

        /// <inheritdoc />
        public EventHandler<LeagueEvent> MessageReceived { get; set; }

        /// <inheritdoc />
        public EventHandler<string> ErrorReceived { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="LeagueEventHandler"/> class.
        /// </summary>
        /// <param name="port">The league client's port.</param>
        /// <param name="token">The user's Basic authentication token.</param>
        public LeagueEventHandler(int port, string token)
        {
            _subscribers = new Dictionary<string, List<EventHandler<LeagueEvent>>>();
            ChangeSettings(port, token);
        }

        /// <inheritdoc />
        public void Connect()
        {
            _webSocket.Connect();
            _webSocket.Send("[5, \"OnJsonApiEvent\"]");
        }

        /// <inheritdoc />
        public void Disconnect()
        {
            _webSocket.Close();
        }

        /// <inheritdoc />
        public void ChangeSettings(int port, string token)
        {
            _webSocket = new WebSocket($"wss://127.0.0.1:{port}/", "wamp");
            _webSocket.SetCredentials("riot", token, true);
            _webSocket.SslConfiguration.EnabledSslProtocols = SslProtocols.Tls12;
            _webSocket.SslConfiguration.ServerCertificateValidationCallback = (response, cert, chain, errors) => true;

            _webSocket.OnError += OnError;
            _webSocket.OnMessage += OnMessage;
        }

        /// <inheritdoc />
        public void Subscribe(string uri, EventHandler<LeagueEvent> eventHandler)
        {
            if (_subscribers.TryGetValue(uri, out List<EventHandler<LeagueEvent>> eventHandlers))
                eventHandlers.Add(eventHandler);
            else
                _subscribers.Add(uri, new List<EventHandler<LeagueEvent>> { eventHandler });
        }

        /// <inheritdoc />
        public void Unsubscribe(string uri)
        {
            if (_subscribers.ContainsKey(uri))
                _subscribers.Remove(uri);
        }

        /// <inheritdoc />
        public void UnsubscribeAll()
        {
            _subscribers.Clear();
        }

        /// <summary>
        /// Called when an error is received from the client.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The error arguments.</param>
        private void OnError(object sender, ErrorEventArgs e)
        {
            ErrorReceived?.Invoke(sender, e.Message);
        }

        /// <summary>
        /// Called when a message is received from the client.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The message arguments.</param>
        private void OnMessage(object sender, MessageEventArgs e)
        {
            // Check if the message is json received from the client
            if (e.IsText)
            {
                var eventArray = JArray.Parse(e.Data);
                var eventNumber = eventArray[0].ToObject<int>();

                if (eventNumber == ClientEventNumber)
                {
                    var leagueEvent = eventArray[ClientEventData].ToObject<LeagueEvent>();
                    MessageReceived?.Invoke(sender, leagueEvent);

                    // Call subscribers
                    if (_subscribers.TryGetValue(leagueEvent.Uri, out List<EventHandler<LeagueEvent>> eventHandlers))
                    {
                        foreach (var eventHandler in eventHandlers)
                            eventHandler?.Invoke(sender, leagueEvent);
                    }
                }
            }
        }
    }
}
