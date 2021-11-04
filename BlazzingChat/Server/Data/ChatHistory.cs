using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BlazzingChat.Server.Data
{
    public partial class ChatHistory
    {
        public int Id { get; set; }

        //[Required]
        //public User FromUser { get; set; }
        public int FromUserId { get; set; }

        //[Required]
        //public User ToUser { get; set; }
        public int ToUserId { get; set; }

        [Required]
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
