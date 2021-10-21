using System;
using System.Collections.Generic;

#nullable disable

namespace BlazzingChat.Server.Data
{
    public partial class ChatHistory
    {
        public int Id { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual User FromUser { get; set; }
        public virtual User ToUser { get; set; }
    }
}
