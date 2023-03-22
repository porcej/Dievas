using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Dievas.Models.Auth;

namespace Dievas.Controllers {

    /// <summary>
    ///     Controller Class <c>AuthController</c> Provids an API to 
    ///     authenticate users and provides them with a session token
    ///     
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {

        /// <summary>
        ///     Application configuration for this Class
        /// </summary>
        private readonly IConfiguration _config;

        /// <summary>
        ///     Logging Controller for this class
        /// </summary>
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        ///     Default constructor for Class <c>AuthController</c>
        /// </summary>
        /// <param name="configuration">IConfiguration configuration informaiton</param>
        /// <param name="logger">ILogger: aggregate logger</param>
        public AuthController(IConfiguration configuration,
                              ILogger<AuthController> logger) {
            _config = configuration;
            _logger = logger;
        }

        /// <summary>
        ///     Listen for loggin requests via post
        /// </summary>
        /// <param name="userLogin">UserLogin user information to faciliate login</param>
        /// <returns> Returns JWT Token is user is authorized and 401 otherwise.</returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login([FromBody] UserLogin userLogin) {
            var user = Authenticate(userLogin);
            if (user != null) {
                _logger.LogInformation($"User {userLogin.Username} authenticated at {DateTime.Now.ToString()}");
                var token = GenerateToken(user);
                return Ok(token);
            }
            _logger.LogInformation($"Failed autentication using uername {userLogin.Username} at {DateTime.Now.ToString()}");
            return Unauthorized();
        }

        /// <summary>
        ///     Creates JWT Token
        /// </summary>
        /// <param name="user">UserModel user information to faciliate token generation</param>
        /// <returns> Returns JWT Token for user with rolls specified by model.</returns>
        private string GenerateToken(UserModel user) {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier,user.Username));
            foreach (string role in user.Roles) {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        ///    Authenticate user
        /// </summary>
        /// <param name="userLogin">UserLogin user information to faciliate authentication</param>
        /// <returns> Returns a UserModel for userLogin if authenticated, null otherwise.</returns>
        private UserModel Authenticate(UserLogin userLogin) {
            bool isValid = false; 

            string domain = _config["Auth:Domain"];
            string ldapUrl = _config["Auth:Server"];

            isValid = validateUserViaLDAP(userLogin.Username, userLogin.Password, domain, ldapUrl);

            if (isValid) {
                UserModel user = Access.Users.FirstOrDefault(x => x.Username == userLogin.Username);
                if (user == null) {
                    user = new UserModel {
                        Username = userLogin.Username,
                        Roles = new List<string> {"user"}
                    };
                }
                return user;
            }
            return null;
        }

        /// <summary>
        /// Attempts to validate a user against a LDAP via bind.  In this case, we 
        /// query the provided LDAP server's database to validate the credentials.
        /// The server defines how a user is mapped to its directory.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="domain">Domain</param>
        /// <param name="ldapUrl">URL for LDAP server including port</param>
        /// <returns>true if the credentials are valid, false otherwise</returns>
        public bool validateUserViaLDAP(string username, string password, string domain, string ldapUrl) {
            var credentials = new NetworkCredential(username, password, domain);
            var serverId = new LdapDirectoryIdentifier(ldapUrl);
            var connection = new LdapConnection(serverId, credentials);
            bool result = true;
            try {
                connection.Bind();
            }
            catch (Exception) {
                result = false;
            }
            connection.Dispose();
            return result;
        }
    }
}