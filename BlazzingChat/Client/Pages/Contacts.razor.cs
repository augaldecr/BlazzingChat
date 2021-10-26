using BlazzingChat.Client.Shared;
using BlazzingChat.Client.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using System;
using System.Threading.Tasks;

namespace BlazzingChat.Client.Pages
{

    public partial class Contacts : ComponentBase
    {
        [CascadingParameter] Task<AuthenticationState> _authenticationState { get; set; }
        [CascadingParameter] Error Error { get; set; }
        [Inject] NavigationManager _navigationManager { get; set; }
        [Inject] IContactsViewModel _contactsViewModel { get; set; }

        public int ContactsCount { get; set; }

        private async ValueTask<ItemsProviderResult<Contact>> LoadOnlyVisibleContacts(ItemsProviderRequest itemsProviderRequest)
        {
            //var results = await _contactsViewModel.GetOnlyVisibleContacts(itemsProviderRequest.StartIndex, itemsProviderRequest.Count);
            var results = await _contactsViewModel.GetVisibleContacts(itemsProviderRequest.StartIndex, itemsProviderRequest.Count);
            
            return new ItemsProviderResult<Contact>(results, ContactsCount);
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var authState = await _authenticationState;
                var user = authState.User;

                if (user.Identity.IsAuthenticated)
                {
                    ContactsCount = await _contactsViewModel.GetContactsCount();
                }
                else _navigationManager.NavigateTo("/");
            }
            catch (Exception ex)
            {
                Error.ProcessError(ex);
            }
        }

        private void NavigateToChat() => _navigationManager.NavigateTo("/chat");
    }
}
