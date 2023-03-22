using System.Collections.Generic;

namespace Dievas.Models.Auth {
    public class UserModel {
        public string Username { get; set; }
        public List<string> Roles { get; set; }
    }
}