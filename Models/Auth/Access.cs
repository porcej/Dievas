using System.Collections.Generic;
using Dievas.Models.Auth;

namespace Dievas.Models.Auth {

    /// <summary>
    ///     Class <c>Access</c> User role list
    /// </summary>
    public class Access {

        /// <summary>
        ///     List of user's and their access level
        /// </summary>
        public static List<UserModel> Users = new List<UserModel>();

        /// <summary>
        ///     Roles assigned to user
        /// </summary>
        public static List<string> Roles = new List<string> { "admin", "messenger", "user" };
    }
}