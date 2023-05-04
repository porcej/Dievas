using System;
using System.Collections.Generic;

namespace Dievas.Models.Auth {
    public class UserModel {
        public string Username { get; set; }
        public List<string> Roles { get; set; }

        public override bool Equals(Object obj) {
            return (obj is UserModel) && ((UserModel)obj).Username == Username;
        }

        public override int GetHashCode() {
            return Username.GetHashCode();
        }
    }
}