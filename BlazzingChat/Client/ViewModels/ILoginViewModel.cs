using BlazzingChat.Shared.Models;
using System.Threading.Tasks;

namespace BlazzingChat.Client.ViewModels
{
    public interface ILoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        bool RememberMe { get; set; }

        Task<AuthenticationResponse> AuthenticateJWT();
        Task<string> GetFacebookAppIDAsync();
        public Task LoginUser();
    }
}