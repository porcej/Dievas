using System.Collections.Generic;
using Dievas.Models.Auth;

namespace Dievas.Models.Auth {

    /// <summary>
    ///     Class <c>Access</c> User role list
    /// </summary>
    public class AuthenticateResponse {

        /// <summary>
        ///     Username for user
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     Auth Token for user
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        ///     Roles assigned to user
        /// </summary>
        public List<string> Roles = new List<string>();

        /// <summary>
        ///     Default Constructor
        /// </summary>
        public AuthenticateResponse(string username, List<string> roles, string token) {
            Username = username;
            Roles = roles;
            Token = token;
        }

        /// <summary>
        ///     UserModel constructor
        /// </summary>
        public AuthenticateResponse(UserModel user, string token) {
            Username = user.Username;
            Roles = user.Roles;
            Token = token;
        }
    }
}