using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazzingChat.Client.ViewModels
{
    public interface IContactsViewModel
    {
        public List<Contact> Contacts { get; set; }
        public Task GetContacts();
        Task<int> GetContactsCount();
        public Task<List<Contact>> GetOnlyVisibleContacts(int startIndex, int count);
        Task<List<Contact>> GetVisibleContacts(int startIndex, int count);
    }
}
