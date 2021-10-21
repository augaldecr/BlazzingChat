using BlazzingChat.Client.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazzingChat.Client.Pages
{

    public partial class Contacts : ComponentBase
    {
        private readonly IContactsViewModel _contactsViewModel;

        public Contacts(IContactsViewModel contactsViewModel)
        {
            _contactsViewModel = contactsViewModel;
        }
        [Inject] NavigationManager NavigationManager { get; set; }
        private List<ViewModels.Contact> ContactList { get; set; } = new();

        protected override async Task OnInitializedAsync() => await _contactsViewModel.GetContacts();

        private void NavigateToChat()
        {
            NavigationManager.NavigateTo("/chat");
        }
    }
}
