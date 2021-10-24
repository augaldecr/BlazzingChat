using BlazzingChat.Shared.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazzingChat.Client.ViewModels
{
    public class ProfileViewModel : IProfileViewModel
    {
        private readonly HttpClient _httpClient;

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string AboutMe { get; set; }

        public string ProfilePictDataUrl { get; set; }

        public string Message { get; set; }
        
        public ProfileViewModel() { }
                
        public ProfileViewModel(HttpClient httpClient) => _httpClient = httpClient;


        public async Task UpdateProfile()
        {
            await _httpClient.PutAsJsonAsync<User>($"api/Users/{Id}", this);
            Message = "Profile updated";
        }

        public async Task GetProfile()
        {
            User user = await _httpClient.GetFromJsonAsync<User>($"api/Users/{Id}");
            LoadData(user);
            Message = "Profile loaded";
        }

        private void LoadData(ProfileViewModel model)
        {
            Id = model.Id;
            FirstName = model.FirstName;
            LastName = model.LastName;
            EmailAddress = model.EmailAddress;
            AboutMe = model.AboutMe;
            ProfilePictDataUrl = model.ProfilePictDataUrl;
        }

        public static implicit operator ProfileViewModel(User user)
        {
            return new()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.EmailAddress,
                AboutMe = user.AboutMe,
                ProfilePictDataUrl=user.ProfilePictDataUrl,
            };
        }

        public static implicit operator User(ProfileViewModel model)
        {
            return new()
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailAddress = model.EmailAddress,
                AboutMe = model.AboutMe,
                ProfilePictDataUrl = model.ProfilePictDataUrl,
            };
        }
    }
}
