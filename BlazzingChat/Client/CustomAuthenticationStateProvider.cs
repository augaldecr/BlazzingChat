using Blazored.LocalStorage;
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
        private readonly ILocalStorageService _localStorageService;

        public CustomAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //User currentUser = await _httpClient.GetFromJsonAsync<User>("api/Users/getcurrentuser");
            User currentUser = await GetUserByJWTAsync();

            if (currentUser is not null && currentUser.EmailAddress is not null)
            {
                //create a claim
                Claim claimEmail = new(ClaimTypes.Name, currentUser.EmailAddress);
                Claim claimIdentifier = new(ClaimTypes.NameIdentifier, currentUser.Id.ToString());
                Claim claimRole = new(ClaimTypes.Role, currentUser.Role ?? string.Empty);
                //create a claimsIdentity
                ClaimsIdentity claimsIdentity = new(new[] { claimEmail, claimIdentifier, claimRole }, "serverAuth");
                //create a claimsPrincipal
                ClaimsPrincipal claimsPrincipal = new(claimsIdentity);

                return new AuthenticationState(claimsPrincipal);
            }
            else
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        public async Task<User> GetUserByJWTAsync()
        {
            //pulling the token from localStorage
            var jwtToken = await _localStorageService.GetItemAsStringAsync("jwt_token");
            if (jwtToken == null) return null;

            //preparing the http request
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "api/Users/getuserbyjwt");
            requestMessage.Content = new StringContent(jwtToken);

            requestMessage.Content.Headers.ContentType
                = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            //making the http request
            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode;
            var returnedUser = await response.Content.ReadFromJsonAsync<User>();

            //returning the user if found
            if (returnedUser != null) return await Task.FromResult(returnedUser);
            else return null;
        }
    }
}
