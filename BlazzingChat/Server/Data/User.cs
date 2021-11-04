using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BlazzingChat.Server.Data
{
    public partial class User
    {
        public int Id { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string Password { get; set; }
        public string Source { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Birthday { get; set; }
        public string AboutMe { get; set; }
        public int? Notifications { get; set; }
        public int? DarkTheme { get; set; }
        public byte[] CreationDate { get; set; }
        public string ProfilePictDataUrl { get; set; }
        public string Role { get; set; }

        public ICollection<ChatHistory> ChatHistoryFromUsers { get; set; }
        public ICollection<ChatHistory> ChatHistoryToUsers { get; set; }
    }
}
