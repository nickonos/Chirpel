using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirpel.Common.Models
{
    public class UserSettings
    {
        public Guid UserId { get; set; }
        public bool DarkModeEnabled { get; set; }
        public bool IsPrivate { get; set; }
        public string Bio { get; set; }
        public string ProfilePicture { get; set; }
    }
}
