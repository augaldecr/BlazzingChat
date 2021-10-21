using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazzingChat.Client.ViewModels
{
    public interface IContactsViewModel
    {
        public List<Contact> Contacts { get; set; }
        public Task GetContacts();
    }
}
