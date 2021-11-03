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
        public bool RememberMe { get; set; }

        public async Task LoginUser()
        {
            await _httpClient.PostAsJsonAsync<User>($"api/Users/loginuser?isPersistent={this.RememberMe}", this);
        }

        public async Task<AuthenticationResponse> AuthenticateJWT()
        {
            //creating authentication request
            AuthenticationRequest authenticationRequest = new AuthenticationRequest();
            authenticationRequest.EmailAddress = this.Email;
            authenticationRequest.Password = this.Password;

            //authenticating the request
            var httpMessageReponse = await _httpClient.PostAsJsonAsync<AuthenticationRequest>($"api/Users/authenticatejwt", authenticationRequest);

            //sending the token to the client to store
            return await httpMessageReponse.Content.ReadFromJsonAsync<AuthenticationResponse>();
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
