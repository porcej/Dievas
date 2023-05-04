using System;
using System.Collections.Generic;

namespace Dievas.Models.Auth {

    /// <summary>
    ///     Class <c>UserModel</c> Model for a user of this application
    /// </summary>
    public class UserModel {

        /// <summary>
        ///     Username for user
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     Roles assigned to user
        /// </summary>
        public List<string> Roles { get; set; }

        /// <summary>
        ///     Username for user
        /// </summary>
        /// <returns> Returns true, iff the compared to a UserModel object with the same username.  Returns false otherwise</returns>
        public override bool Equals(Object obj) {
            return (obj is UserModel) && ((UserModel)obj).Username == Username;
        }

        /// <summary>
        ///     Returns has code for this object
        /// </summary>
        /// <returns> Hash code for this object</returns>
        public override int GetHashCode() {
            return Username.GetHashCode();
        }
    }
}