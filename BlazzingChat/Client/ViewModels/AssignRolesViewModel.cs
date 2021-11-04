using Blazored.Toast.Services;
using BlazzingChat.Shared.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazzingChat.Client.ViewModels
{
    public class AssignRolesViewModel : IAssignRolesViewModel
    {
        //properties
        public List<User> UsersWithoutRole { get; set; }

        private HttpClient _httpClient;
        public IToastService _toastService { get; }

        //methods
        public AssignRolesViewModel()
        {
        }
        public AssignRolesViewModel(HttpClient httpClient, IToastService toastService)
        {
            _httpClient = httpClient;
            _toastService = toastService;
        }

        public async Task LoadUsersWithoutRole()
        {
            UsersWithoutRole = await _httpClient.GetFromJsonAsync<List<User>>($"api/Users/getuserswithoutrole");
        }
        public async Task AssignRole(int userId, string role)
        {
            var user = new User { Id = userId, Role = role };
            await _httpClient.PutAsJsonAsync("api/Users/assignrole", user);
        }

        public async Task DeleteUser(long userId)
        {
            await _httpClient.DeleteAsync("api/Users/deleteuser/" + userId);
        }
    }
}
