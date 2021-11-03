using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazzingChat.Shared.Models;

namespace BlazzingChat.Client.ViewModels
{
    public class RegisterViewModel : IRegisterViewModel
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string ReenterPassword { get; set; }

        private HttpClient _httpClient;
        public RegisterViewModel() { }
        public RegisterViewModel(HttpClient httpClient) => _httpClient = httpClient;

        public async Task RegisterUser()
        {
            Console.WriteLine(EmailAddress);
            Console.WriteLine(Password);
            await _httpClient.PostAsJsonAsync<User>("api/Users/registeruser", this);
        }

        public static implicit operator RegisterViewModel(User user)
        {
            return new RegisterViewModel
            {
                EmailAddress = user.EmailAddress,
                Password = user.Password
            };
        }

        public static implicit operator User(RegisterViewModel registerViewModel)
        {
            return new User
            {
                EmailAddress = registerViewModel.EmailAddress,
                Password = registerViewModel.Password
            };
        }
    }
}
