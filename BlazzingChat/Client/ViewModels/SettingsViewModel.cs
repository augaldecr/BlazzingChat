using BlazzingChat.Shared.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazzingChat.Client.ViewModels
{
    public class SettingsViewModel : ISettingsViewModel
    {
        public int Id { get; set; }
        public bool Notifications { get; set; }
        public bool DarkTheme { get; set; }

        private HttpClient _httpClient;

        public SettingsViewModel()
        {
        }
        public SettingsViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task GetProfile()
        {
            User user = await _httpClient.GetFromJsonAsync<User>($"api/Users/{Id}");
            LoadCurrentObject(user);
        }
        public async Task Save()
        {
            User user = new() { DarkTheme = int.Parse(this.DarkTheme.ToString()), Notifications = int.Parse(this.Notifications.ToString()) };
            await _httpClient.PutAsJsonAsync<User>($"api/Users/UpdateSettings/{Id}", this);
        }
        private void LoadCurrentObject(SettingsViewModel settingsViewModel)
        {
            Id = settingsViewModel.Id;
            DarkTheme = settingsViewModel.DarkTheme;
            Notifications = settingsViewModel.Notifications;
        }

        public static implicit operator SettingsViewModel(User user)
        {
            return new SettingsViewModel
            {
                Id = user.Id,
                Notifications = (user.Notifications == null || (int)user.Notifications == 0) ? false : true,
                DarkTheme = (user.DarkTheme == null || (int)user.DarkTheme == 0) ? false : true
            };
        }
        public static implicit operator User(SettingsViewModel settingsViewModel)
        {
            return new User
            {
                Id = settingsViewModel.Id,
                Notifications = settingsViewModel.Notifications ? 1 : 0,
                DarkTheme = settingsViewModel.DarkTheme ? 1 : 0
            };
        }
    }
}
