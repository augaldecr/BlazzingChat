using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace BlazzingChat.Client.Logging
{
    public class ApplicationLoggerProvider : ILoggerProvider
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public ApplicationLoggerProvider(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new DatabaseLogger(_httpClient, _authenticationStateProvider);
        }

        public void Dispose()
        {

        }
    }
}
