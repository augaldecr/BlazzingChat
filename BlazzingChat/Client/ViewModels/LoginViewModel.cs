using BlazzingChat.Shared.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazzingChat.Client.ViewModels
{
    public class LoginViewModel : ILoginViewModel
    {
        private readonly HttpClient _httpClient;

        public LoginViewModel() { }

        public LoginViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public string Email { get; set; }
        public string Password { get; set; }

        public async Task LoginUser()
        {
            await _httpClient.PostAsJsonAsync<User>("api/Users/loginuser", this);
        }

        public static implicit operator LoginViewModel(User user)
        {
            return new LoginViewModel
            {
                Email = user.EmailAddress,
                Password = user.Password,
            };
        }

        public static implicit operator User(LoginViewModel loginViewModel)
        {
            return new User
            {
                EmailAddress = loginViewModel.Email,
                Password = loginViewModel.Password,
            };
        }
    }
}
