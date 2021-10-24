using BlazzingChat.Shared.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazzingChat.Client.ViewModels
{
    public class ContactsViewModel : IContactsViewModel
    {
        //properties
        public List<Contact> Contacts { get; set; }
        private HttpClient _httpClient;

        //methods
        public ContactsViewModel()
        {
        }
        public ContactsViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task GetContacts()
        {
            List<User> users = await _httpClient.GetFromJsonAsync<List<User>>("api/Users");
            LoadCurrentObject(users);
        }

        public async Task<List<Contact>> GetOnlyVisibleContacts(int startIndex, int count)
        {
            List<User> users = await _httpClient.GetFromJsonAsync<List<User>>(
                $"api/Users/getonlyvisiblecontacts?startIndex={startIndex}&count={count}");
            LoadCurrentObject(users);
            return Contacts;
        }

        private void LoadCurrentObject(List<User> users)
        {
            this.Contacts = new List<Contact>();
            foreach (User user in users)
            {
                this.Contacts.Add(user);
            }
        }
    }
}
