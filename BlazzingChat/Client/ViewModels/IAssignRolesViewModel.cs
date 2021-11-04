using Blazored.Toast.Services;
using BlazzingChat.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazzingChat.Client.ViewModels
{
    public interface IAssignRolesViewModel
    {
        IToastService _toastService { get; }
        List<User> UsersWithoutRole { get; set; }

        Task AssignRole(int userId, string role);
        Task DeleteUser(long userId);
        Task LoadUsersWithoutRole();
    }
}