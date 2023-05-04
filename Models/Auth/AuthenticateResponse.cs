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
        ///     Default Constructor
        /// </summary>
        public AuthenticateResponse(string username, string token) {
            Username = username;
            Token = token;
        }
    }
}