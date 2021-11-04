using System.Threading.Tasks;

namespace BlazzingChat.Client.ViewModels
{
    public interface IFacebookAuthViewModel
    {
        Task<string> GetFacebookJWTAsync(string accessToken);
    }
}