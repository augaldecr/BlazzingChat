using BlazzingChat.Server.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazzingChat.Server.Logging
{
    public class DatabaseLogger : ILogger
    {
        private readonly BlazzingChatDbContext _blazzingChatDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DatabaseLogger(BlazzingChatDbContext blazzingChatDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _blazzingChatDbContext = blazzingChatDbContext;
            _httpContextAccessor = httpContextAccessor;
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
            var userId = _httpContextAccessor.HttpContext is null ?
                string.Empty :
                _httpContextAccessor?.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) is null ?
                    String.Empty :
                        _httpContextAccessor?.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value;


            Log log = new();
            log.LogLevel = logLevel.ToString();
            log.EventName = eventId.ToString();
            log.ExceptionMessage = exception?.Message;
            log.StackTrace = exception?.StackTrace;
            log.Source = "Server";
            log.CreatedDate = DateTime.Now.ToString();
            log.UserId = string.IsNullOrEmpty(userId) ? null : Convert.ToInt32(userId);

            _blazzingChatDbContext.Logs.Add(log);

            try
            {
                _blazzingChatDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
