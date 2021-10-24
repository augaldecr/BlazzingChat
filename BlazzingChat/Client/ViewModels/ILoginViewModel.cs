using System.Threading.Tasks;

namespace BlazzingChat.Client.ViewModels
{
    public interface ILoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public Task LoginUser();
    }
}