using Blazored.LocalStorage;
using Blazored.Toast.Services;
using BlazzingChat.Client.ViewModels;
using BlazzingChat.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;

namespace BlazzingChat.Client.Pages
{
    public partial class Login : ComponentBase
    {
        [Inject] private ILoginViewModel _loginViewModel { get; set; }
        [Inject] private NavigationManager _navigationManager { get; set; }
        [Inject] private ILocalStorageService _localStorageService { get; set; }
        [Inject] private IToastService _toastService { get; set; }

        [CascadingParameter] public Task<AuthenticationState> authenticationState { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await authenticationState;
            var user = authState.User;
            if (user.Identity.IsAuthenticated)
            {
                _navigationManager.NavigateTo("/profile", true);
            }
        }

        private async Task LoginUser()
        {
            await _loginViewModel.LoginUser();
            _navigationManager.NavigateTo("/profile", true);
        }

        private void TwitterSignIn()
        {
            _navigationManager.NavigateTo($"api/Users/TwitterSignIn?isPersistent={_loginViewModel.RememberMe}", true);
        }       
        
        private void FacebookSignIn()
        {
            _navigationManager.NavigateTo($"api/Users/FacebookSignIn?isPersistent={_loginViewModel.RememberMe}", true);
        } 
        
        private void GoogleSignIn()
        {
            _navigationManager.NavigateTo($"api/Users/GoogleSignIn?isPersistent={_loginViewModel.RememberMe}", true);
        }

        public async Task AuthenticateJWT()
        {
            AuthenticationResponse authenticationResponse = await _loginViewModel.AuthenticateJWT();
            if (!string.IsNullOrEmpty(authenticationResponse.Token))
            {
                await _localStorageService.SetItemAsync("jwt_token", authenticationResponse.Token);
                _navigationManager.NavigateTo("/profile", true);
            }
            else
            {
                _toastService.ShowError("Invalid username or password");
            }
        }

        public async Task FacebookJWT()
        {
            var appId = await _loginViewModel.GetFacebookAppIDAsync();

            var accessTokenRequest = $"https://www.facebook.com/v11.0/dialog/oauth?&response_type=token&client_id={appId}&redirect_uri=https://localhost:5001/FacebookAuth";
            _navigationManager.NavigateTo(accessTokenRequest, true);
        }
    }
}
