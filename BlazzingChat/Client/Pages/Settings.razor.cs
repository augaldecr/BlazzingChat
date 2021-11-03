using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;
using BlazzingChat.Client.ViewModels;
using System.Security.Claims;
using System;
using BlazzingChat.Client.Shared;
using Microsoft.JSInterop;

namespace BlazzingChat.Client.Pages
{
    public partial class Settings : ComponentBase
    {
        [CascadingParameter] public Task<AuthenticationState> authenticationState { get; set; }
        [CascadingParameter] public Error Error { get; set; }
        [Inject] ISettingsViewModel _settingsViewModel { get; set; }
        [Inject] NavigationManager _navigationManager { get; set; }
        [Inject] IJSRuntime _jsRuntime { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var authState = await authenticationState;//_authenticationStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;

                if (user.Identity.IsAuthenticated)
                {
                    var claim = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
                    _settingsViewModel.Id = Convert.ToInt32(claim?.Value);
                    await _settingsViewModel.GetProfile();
                }
                else
                {
                    _navigationManager.NavigateTo("/");
                }
            }
            catch (Exception ex)
            {
                Error.ProcessError(ex);
            }
        }

        private async Task UpdateTheme()
        {
            var themeName = _settingsViewModel.DarkTheme ? "dark" : "light";

            var module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/site.js");
            await module.InvokeVoidAsync("setTheme", themeName);

            await _settingsViewModel.UpdateTheme();
            StateHasChanged();
        }
        private async Task UpdateNotifications()
        {
            await _settingsViewModel.UpdateNotifications();
        }
    }
}
