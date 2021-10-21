using System.Threading.Tasks;

namespace BlazzingChat.Client.ViewModels
{
    public interface IProfileViewModel
    {
        string EmailAddress { get; set; }
        string FirstName { get; set; }
        int Id { get; set; }
        string LastName { get; set; }
        string Message { get; set; }
        Task GetProfile();
        Task UpdateProfile();
    }
}