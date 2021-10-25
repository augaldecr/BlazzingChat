using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;
using BlazzingChat.Client.ViewModels;
using System.Security.Claims;
using System;

namespace BlazzingChat.Client.Pages
{
    public partial class Settings : ComponentBase
    {
        [CascadingParameter] public Task<AuthenticationState> authenticationState { get; set; }
        [Inject] ISettingsViewModel _settingsViewModel { get; set; }
        [Inject] NavigationManager _navigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await authenticationState;//_authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                var claim = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
                _settingsViewModel.Id = Convert.ToInt32(claim?.Value);
                throw new NullReferenceException();
                await _settingsViewModel.GetProfile();
            }
            else
            {
                _navigationManager.NavigateTo("/");
            }
        }
    }
}
