using BlazzingChat.Client.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazzingChat.Client.Pages
{

    public partial class Contacts : ComponentBase
    {
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] IContactsViewModel _contactsViewModel { get; set; }
        private async ValueTask<ItemsProviderResult<Contact>> LoadOnlyVisibleContacts(ItemsProviderRequest itemsProviderRequest)
        {
            var results = await _contactsViewModel.GetOnlyVisibleContacts(itemsProviderRequest.StartIndex, itemsProviderRequest.Count);
            return new ItemsProviderResult<Contact>(results, 40000);
        }
        protected override async Task OnInitializedAsync() => await _contactsViewModel.GetContacts();

        private void NavigateToChat()
        {
            NavigationManager.NavigateTo("/chat");
        }
    }
}
