using System.Threading.Tasks;

namespace BlazzingChat.Client.ViewModels
{
    public interface IProfileViewModel
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AboutMe { get; set; }
        public string Message { get; set; }
        string ProfilePictDataUrl { get; set; }

        public Task GetProfile();
        public Task UpdateProfile();
    }
}