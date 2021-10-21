using BlazzingChat.Client.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BlazzingChat.Client.Pages
{
    public partial class Profile : ComponentBase
    {
        [Inject] IProfileViewModel _profileViewModel { get; set; }

        protected override async Task OnInitializedAsync() => await _profileViewModel.GetProfile();
    }
}
