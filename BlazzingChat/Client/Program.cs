using BlazzingChat.Client.Logging;
using BlazzingChat.Client.ViewModels;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazzingChat.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddLogging(logging =>
            {
                var httpClient = builder.Services.BuildServiceProvider().GetRequiredService<HttpClient>();
                logging.SetMinimumLevel(LogLevel.Error);
                logging.AddProvider(new ApplicationLoggerProvider(httpClient));
            });

            LoadHttpClients(builder);
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

            await builder.Build().RunAsync();
        }

        public static void LoadHttpClients(WebAssemblyHostBuilder builder)
        {
            builder.Services.AddHttpClient<IProfileViewModel, ProfileViewModel>
                ("BlazzingChatClient", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            builder.Services.AddHttpClient<ISettingsViewModel, SettingsViewModel>
                ("BlazzingChatClient", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            builder.Services.AddHttpClient<IContactsViewModel, ContactsViewModel>
                ("BlazzingChatClient", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            builder.Services.AddHttpClient<ILoginViewModel, LoginViewModel>
                ("BlazzingChatClient", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
        }
    }
}
