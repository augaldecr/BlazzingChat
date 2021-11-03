using Blazored.LocalStorage;
using BlazzingChat.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazzingChat.Client.Shared
{
    public partial class MainLayout
    {
        [Inject] private NavigationManager _navigationManager { get; set; }
        [Inject] private HttpClient _httpClient { get; set; }
        [Inject] private ILocalStorageService _localStorageService { get; set; }
        [Inject] IJSRuntime _jsRuntime { get; set; }

        protected override async Task OnInitializedAsync()
        {
            User user = await _httpClient.GetFromJsonAsync<User>($"api/Users/getcurrentuser");

            if (user is not null)
            {
                var themeName = user.DarkTheme == 1 ? "dark" : "light";

                var module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/site.js");
                await module.InvokeVoidAsync("setTheme", themeName);
                StateHasChanged();
            }
        }

        private async Task LogoutUser()
        {
            //await _httpClient.GetAsync("api/Users/logoutuser");
            await _localStorageService.RemoveItemAsync("jwt_token");
            _navigationManager.NavigateTo("/", true);
        }

        private void LoginUser()
        {
            _navigationManager.NavigateTo("/", true);
        }
    }
}
