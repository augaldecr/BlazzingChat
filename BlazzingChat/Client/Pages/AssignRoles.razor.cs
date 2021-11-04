using Blazored.Toast.Services;
using BlazzingChat.Client.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace BlazzingChat.Client.Pages
{
    public partial class AssignRoles : ComponentBase
    {
        //[Authorize(Roles = "admin")]
        [Inject] public IAssignRolesViewModel _assignRolesViewModel { get; set; }
        [Inject] public IToastService _toastService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await _assignRolesViewModel.LoadUsersWithoutRole();
        }

        private async Task DeleteUser(long userId)
        {
            await _assignRolesViewModel.DeleteUser(userId);
            await _assignRolesViewModel.LoadUsersWithoutRole();
            StateHasChanged();
        }

        private async Task AssignRole(ChangeEventArgs eventArgs)
        {
            Console.WriteLine(eventArgs.Value);
            var selectedValues = eventArgs.Value.ToString().Split('#');

            await _assignRolesViewModel.AssignRole(Convert.ToInt32(selectedValues[1]), selectedValues[0]);
            _toastService.ShowSuccess("Role updated successfully");
        }
    }
}
