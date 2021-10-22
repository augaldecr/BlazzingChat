using BlazzingChat.Shared.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazzingChat.Client.ViewModels
{
    public class SettingsViewModel : ISettingsViewModel
    {
        //properties
        public bool Notifications { get; set; }
        public bool DarkTheme { get; set; }

        private HttpClient _httpClient;

        //methods
        public SettingsViewModel()
        {
        }
        public SettingsViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task GetProfile()
        {
            User user = await _httpClient.GetFromJsonAsync<User>("api/Users/1");
            LoadCurrentObject(user);
        }
        public async Task Save()
        {
            User user = new() { DarkTheme = int.Parse(this.DarkTheme.ToString()), Notifications = int.Parse(this.Notifications.ToString()) };
            await _httpClient.PutAsJsonAsync<User>($"api/Users/UpdateSettings/1", this);
        }
        private void LoadCurrentObject(SettingsViewModel settingsViewModel)
        {
            this.DarkTheme = settingsViewModel.DarkTheme;
            this.Notifications = settingsViewModel.Notifications;
        }

        //operators
        public static implicit operator SettingsViewModel(User user)
        {
            return new SettingsViewModel
            {
                Notifications = (user.Notifications == null || (int)user.Notifications == 0) ? false : true,
                DarkTheme = (user.DarkTheme == null || (int)user.DarkTheme == 0) ? false : true
            };
        }
        public static implicit operator User(SettingsViewModel settingsViewModel)
        {
            return new User
            {
                Notifications = settingsViewModel.Notifications ? 1 : 0,
                DarkTheme = settingsViewModel.DarkTheme ? 1 : 0
            };
        }
    }
}
