using Blazored.LocalStorage;
using BlazzingChat.Client.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System.Threading.Tasks;

namespace BlazzingChat.Client.Pages
{
    public partial class FacebookAuth : ComponentBase
    {
        [Inject] public NavigationManager _navigationManager { get; set; }
        [Inject] public IFacebookAuthViewModel _facebookViewModel { get; set; }
        [Inject] public ILocalStorageService _localStorageService { get; set; }

        public string AccessToken { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (QueryHelpers.ParseQuery(_navigationManager.Uri.Split('#')[1]).TryGetValue("access_token", out Microsoft.Extensions.Primitives.StringValues _accessToken))
            {
                AccessToken = _accessToken;

                string token = await _facebookViewModel.GetFacebookJWTAsync(_accessToken);
                await _localStorageService.SetItemAsync<string>("jwt_token", token);

                _navigationManager.NavigateTo("/profile", true);
            }
        }
    }
}
