using BlazzingChat.Shared.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazzingChat.Client.ViewModels
{
    public class FacebookAuthViewModel : IFacebookAuthViewModel
    {
        private readonly HttpClient _httpClient;
        public FacebookAuthViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetFacebookJWTAsync(string accessToken)
        {
            var httpMessageResponse = await _httpClient.PostAsJsonAsync<FacebookAuthRequest>("api/Users/getfacebookjwt", new FacebookAuthRequest() { AccessToken = accessToken });
            return (await httpMessageResponse.Content.ReadFromJsonAsync<AuthenticationResponse>()).Token;
        }
    }
}
