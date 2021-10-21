using Microsoft.AspNetCore.Components;

namespace BlazzingChat.Client.Pages
{
    public partial class Chat
    {
        [Parameter] public int ContactId { get; set; }
    }
}
