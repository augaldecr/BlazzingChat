using BlazzingChat.Client.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BlazzingChat.Client.Pages
{
    public partial class Login : ComponentBase
    {
        [Inject] private ILoginViewModel _loginViewModel { get; set; }
        [Inject] private NavigationManager _navigationManager { get; set; }

        private async Task LoginUser()
        {
            await _loginViewModel.LoginUser();
            _navigationManager.NavigateTo("/profile", true);
        }
    }
}
