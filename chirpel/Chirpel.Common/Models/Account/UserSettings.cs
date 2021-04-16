using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirpel.Common.Models.Account
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
