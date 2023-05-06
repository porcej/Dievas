namespace Dievas.Models.Auth {

    /// <summary>
    ///     Class <c>UserLogin</c> Representation of an authenticaiton and authorization credential set
    /// </summary>
    public class UserLogin {

        /// <summary>
        ///     Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     Password
        /// </summary>
        public string Password { get; set; }
    }
}