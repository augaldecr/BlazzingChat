using BlazzingChat.Server.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BlazzingChat.Server.Logging
{
    public class ApplicationLoggerProvider : ILoggerProvider
    {
        private readonly BlazzingChatDbContext _blazzingChatDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationLoggerProvider(BlazzingChatDbContext blazzingChatDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _blazzingChatDbContext = blazzingChatDbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new DatabaseLogger(_blazzingChatDbContext, _httpContextAccessor);
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}
