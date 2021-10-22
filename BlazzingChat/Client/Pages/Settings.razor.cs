using BlazzingChat.Client.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BlazzingChat.Client.Pages
{
    public partial class Settings : ComponentBase
    {
        [Inject] public ISettingsViewModel _settingsViewModel { get; set; }

        protected override async Task OnInitializedAsync() => await _settingsViewModel.GetProfile();
    }
}
