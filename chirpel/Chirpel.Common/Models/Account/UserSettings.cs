using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Contract.Models.Account
{
    public class UserSettings
    {
        public string Id { get; set; }
        public bool DarkModeEnabled { get; set; }
        public bool IsPrivate { get; set; }
        public string Bio { get; set; }
        public string ProfilePicture { get; set; }
    }
}
