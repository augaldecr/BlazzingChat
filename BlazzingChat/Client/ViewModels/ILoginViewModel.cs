using System.Threading.Tasks;

namespace BlazzingChat.Client.ViewModels
{
    public interface ILoginViewModel
    {
        string Email { get; set; }
        string Password { get; set; }
        Task LoginUser();
    }
}