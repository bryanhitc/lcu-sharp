using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LCUSharp.Http
{
    /// <summary>
    /// A request handler for the league client that requires the client's port and the user's Basic authentication token.
    /// </summary>
    public class LeagueRequestHandler : RequestHandler
    {
        /// <summary>
        /// The league client's port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// The user's Basic authentication token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="LeagueRequestHandler"/> class.
        /// </summary>
        /// <param name="port">The league client's port.</param>
        /// <param name="token">The user's Basic authentication token.</param>
        public LeagueRequestHandler(int port, string token)
        {
            Port = port;
            Token = token;

            var authTokenBytes = Encoding.ASCII.GetBytes($"riot:{token}");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authTokenBytes));
            HttpClient.BaseAddress = new Uri($"https://127.0.0.1:{port}/");
        }

        /// <summary>
        /// Creates and sends a new <see cref="HttpRequestMessage"/> and returns the <see cref="HttpResponseMessage"/>'s content.
        /// </summary>
        /// <param name="httpMethod">The <see cref="HttpMethod"/>.</param>
        /// <param name="relativeUrl">The relative url.</param>
        /// <param name="queryParameters">The query parameters.</param>
        /// <returns>The <see cref="HttpResponseMessage"/>'s content.</returns>
        public async Task<string> GetJsonResponseAsync(HttpMethod httpMethod, string relativeUrl, IEnumerable<string> queryParameters = null)
        {
            return await GetJsonResponseAsync<object>(httpMethod, relativeUrl, queryParameters, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates and sends a new <see cref="HttpRequestMessage"/> and returns the <see cref="HttpResponseMessage"/>'s content.
        /// </summary>
        /// <typeparam name="TRequest">The object to serialize into the body.</typeparam>
        /// <param name="httpMethod">The <see cref="HttpMethod"/>.</param>
        /// <param name="relativeUrl">The relative url.</param>
        /// <param name="queryParameters">The query parameters.</param>
        /// <param name="body">The request's body.</param>
        /// <returns>The <see cref="HttpResponseMessage"/>'s content.</returns>
        public async Task<string> GetJsonResponseAsync<TRequest>(HttpMethod httpMethod, string relativeUrl, IEnumerable<string> queryParameters, TRequest body)
        {
            var request = await PrepareRequestAsync(httpMethod, relativeUrl, queryParameters, body).ConfigureAwait(false);
            var response = await HttpClient.SendAsync(request).ConfigureAwait(false);
            return await GetResponseContentAsync(response).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates and sends a new <see cref="HttpRequestMessage"/> and deserializes the <see cref="HttpResponseMessage"/>'s content (json) as <typeparamref name="TResponse"/>.
        /// </summary>
        /// <typeparam name="TResponse">The object to deserialize the response into.</typeparam>
        /// <param name="httpMethod">The <see cref="HttpMethod"/>/</param>
        /// <param name="relativeUrl">The relative url.</param>
        /// <param name="queryParameters">The query parameters.</param>
        /// <returns>The deserialized response.</returns>
        public async Task<TResponse> GetResponseAsync<TResponse>(HttpMethod httpMethod, string relativeUrl, IEnumerable<string> queryParameters = null)
        {
            return await GetResponseAsync<object, TResponse>(httpMethod, relativeUrl, queryParameters, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates and sends a new <see cref="HttpRequestMessage"/> and deserializes the <see cref="HttpResponseMessage"/>'s content (json) as <typeparamref name="TResponse"/>.
        /// </summary>
        /// <typeparam name="TRequest">The object to serialize into the body.</typeparam>
        /// <typeparam name="TResponse">The object to deserialize the response into.</typeparam>
        /// <param name="httpMethod">The <see cref="HttpMethod"/>/</param>
        /// <param name="relativeUrl">The relative url.</param>
        /// <param name="queryParameters">The query parameters.</param>
        /// <param name="body">The request's body.</param>
        /// <returns>The deserialized response.</returns>
        public async Task<TResponse> GetResponseAsync<TRequest, TResponse>(HttpMethod httpMethod, string relativeUrl, IEnumerable<string> queryParameters, TRequest body)
        {
            var json = await GetJsonResponseAsync(httpMethod, relativeUrl, queryParameters, body).ConfigureAwait(false);
            return await Task.Run(() => JsonConvert.DeserializeObject<TResponse>(json)).ConfigureAwait(false);
        }
    }
}
