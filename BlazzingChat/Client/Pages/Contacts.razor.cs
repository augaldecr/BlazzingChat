using BlazzingChat.Client.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazzingChat.Client.Pages
{

    public partial class Contacts : ComponentBase
    {
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] IContactsViewModel _contactsViewModel { get; set; }
        private List<ViewModels.Contact> ContactList { get; set; } = new();

        protected override async Task OnInitializedAsync() => await _contactsViewModel.GetContacts();

        private void NavigateToChat()
        {
            NavigationManager.NavigateTo("/chat");
        }
    }
}
