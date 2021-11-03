using System.Threading.Tasks;

namespace BlazzingChat.Client.ViewModels
{
    public interface IRegisterViewModel
    {
        string EmailAddress { get; set; }
        string Password { get; set; }
        string ReenterPassword { get; set; }

        Task RegisterUser();
    }
}