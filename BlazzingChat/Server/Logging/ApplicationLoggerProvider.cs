using BlazzingChat.Server.Data;
using Microsoft.Extensions.Logging;

namespace BlazzingChat.Server.Logging
{
    public class ApplicationLoggerProvider : ILoggerProvider
    {
        private readonly BlazzingChatDbContext _blazzingChatDbContext;

        public ApplicationLoggerProvider(BlazzingChatDbContext blazzingChatDbContext)
        {
            _blazzingChatDbContext = blazzingChatDbContext;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new DatabaseLogger(_blazzingChatDbContext);
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}
