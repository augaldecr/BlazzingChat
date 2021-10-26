using BlazzingChat.Shared.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazzingChat.Client.Logging
{
    public class DatabaseLogger : ILogger
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public DatabaseLogger(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Task.Run( async() => {
                var authState =  await _authenticationStateProvider.GetAuthenticationStateAsync();
                var userId = authState is null ? new() : Convert.ToInt32(authState.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                Log log = new();
                log.LogLevel = logLevel.ToString();
                log.EventName = eventId.ToString();
                log.ExceptionMessage = exception?.Message;
                log.StackTrace = exception?.StackTrace;
                log.Source = "Client";
                log.CreatedDate = DateTime.Now.ToString();
                log.UserId = (int?)userId;

                await _httpClient.PostAsJsonAsync<Log>("api/Logs", log);
            });            
        }
    }
}
