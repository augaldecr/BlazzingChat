using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazzingChat.Client.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject] private NavigationManager _navigationManager { get; set; }
        [Inject] private HttpClient _httpClient { get; set; }

        private async Task LogoutUser()
        {
            await _httpClient.GetAsync("api/Users/logoutuser");
            _navigationManager.NavigateTo("/", true);
        }

        private void LoginUser()
        {
            _navigationManager.NavigateTo("/", true);
        }
    }
}
