using BlazzingChat.Server.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BlazzingChat.Server.Logging
{
    public class DatabaseLogger : ILogger
    {
        private readonly BlazzingChatDbContext _blazzingChatDbContext;

        public DatabaseLogger(BlazzingChatDbContext blazzingChatDbContext)
        {
            _blazzingChatDbContext = blazzingChatDbContext;
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
            Log log = new();
            log.LogLevel = logLevel.ToString();
            log.EventName = eventId.ToString();
            log.ExceptionMessage = exception?.Message;
            log.StackTrace = exception?.StackTrace;
            log.Source = "Server";
            log.CreatedDate = DateTime.Now.ToString();

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
