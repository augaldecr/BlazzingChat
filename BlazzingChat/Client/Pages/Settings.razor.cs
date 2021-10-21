using BlazzingChat.Client.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BlazzingChat.Client.Pages
{
    public partial class Settings : ComponentBase
    {
        private readonly ISettingsViewModel _settingsViewModel;

        public Settings(ISettingsViewModel settingsViewModel)
        {
            _settingsViewModel = settingsViewModel;
        }

        protected override async Task OnInitializedAsync() => await _settingsViewModel.GetProfile();
    }
}
