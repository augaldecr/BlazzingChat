using BlazzingChat.Shared.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazzingChat.Client
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;

        public CustomAuthenticationStateProvider(HttpClient httpClient) => _httpClient = httpClient;

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            User currentUser = await _httpClient.GetFromJsonAsync<User>("api/Users/getcurrentuser");

            if (currentUser is not null && currentUser.EmailAddress is not null)
            {
                //create a claim
                Claim claimEmail = new(ClaimTypes.Name, currentUser.EmailAddress);
                Claim claimIdentifier = new(ClaimTypes.NameIdentifier, currentUser.Id.ToString());
                //create a claimsIdentity
                ClaimsIdentity claimsIdentity = new(new[] { claimEmail, claimIdentifier }, "serverAuth");
                //create a claimsPrincipal
                ClaimsPrincipal claimsPrincipal = new(claimsIdentity);

                return new AuthenticationState(claimsPrincipal);
            }
            else
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }
    }
}
