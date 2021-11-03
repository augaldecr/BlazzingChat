using Blazored.Toast.Services;
using BlazzingChat.Client.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BlazzingChat.Client.Pages
{
    public partial class CreateAccount : ComponentBase
    {
        [Inject] NavigationManager _navigationManager { get; set; }
        [Inject] IRegisterViewModel _registerViewModel { get; set; }
        [Inject] IToastService _toastService { get; set; }

        public async Task RegisterUser()
        {
            await _registerViewModel.RegisterUser();
            _toastService.ShowInfo("Your user has been created in the system. Please login with your credentials");
            _navigationManager.NavigateTo("/");
        }

        private void TwitterSignIn()
        {
            _navigationManager.NavigateTo("api/Users/TwitterSignIn", true);
        }

        private void FacebookSignIn()
        {
            _navigationManager.NavigateTo("api/Users/FacebookSignIn", true);
        }

        private void GoogleSignIn()
        {
            _navigationManager.NavigateTo("api/Users/GoogleSignIn", true);
        }
    }
}
