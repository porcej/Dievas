using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.DirectoryServices.AccountManagement;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Dievas.Models.Auth;
using Dievas.Data;

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
        ///     Database Access Object
        /// </summary>
        private AppDbContext _appDbContext;

        /// <summary>
        ///     Default constructor for Class <c>AuthController</c>
        /// </summary>
        /// <param name="configuration">IConfiguration configuration informaiton</param>
        /// <param name="logger">ILogger: aggregate logger</param>
        public AuthController(IConfiguration configuration,
                              ILogger<AuthController> logger,
                              AppDbContext appDbContext) {
            _config = configuration;
            _logger = logger;
            _appDbContext = appDbContext;
        }

        /// <summary>
        ///     Listen for loggin requests via post
        /// </summary>
        /// <param name="userLogin">UserLogin user information to faciliate login</param>
        /// <returns> Returns JWT Token if user is authorized and 400 otherwise.</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] UserLogin userLogin) {
            var user = await Authenticate(userLogin);
            if (user != null) {
                _logger.LogInformation($"User {userLogin.Username} authenticated at {DateTime.Now.ToString()}");
                var token = GenerateToken(user);
                return Ok(new AuthenticateResponse(user, token));
            }
            _logger.LogInformation($"Failed autentication using uername {userLogin.Username} at {DateTime.Now.ToString()}");
            return BadRequest(new { message = "Username or password is incorrect" });
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
        private async Task<UserModel> Authenticate(UserLogin userLogin) {
            bool isValid = false; 

            string domain = _config["Auth:Domain"];
            string ldapUrl = _config["Auth:Server"];

            isValid = validateUserViaLDAP(userLogin.Username, userLogin.Password, domain, ldapUrl);

            if (isValid) {
                List<string> roles = new List<string>();

                // Check if user is admin
                if (isMemberOfGroup(userLogin.Username, _config["Auth:UserGroups:Admins"], domain))
                    roles.Add("admin");

                // Check if user is an author
                if (isMemberOfGroup(userLogin.Username, _config["Auth:UserGroups:Authors"], domain))
                    roles.Add("author");

                // Check if user is an author
                if (isMemberOfGroup(userLogin.Username, _config["Auth:UserGroups:Approvers"], domain))
                    roles.Add("approver");

                UserModel user = await _appDbContext.Users.FindAsync(userLogin.Username);
                // UserModel user = Access.Users.FirstOrDefault(x => x.Username == userLogin.Username);
                if (user == null) {
                    user = new UserModel {
                        Username = userLogin.Username,
                        Roles = roles
                    };
                    await _appDbContext.Users.AddAsync(user);
                    _logger.LogInformation($"Adding new user {user.Username}.");
                } else {
                    user.Roles = roles;
                }
                await _appDbContext.SaveChangesAsync();
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
        private bool validateUserViaLDAP(string username, string password, string domain, string ldapUrl) {
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

        /// <summary>
        /// Attempts to check if the username is a member of group in domain
        /// </summary>
        /// <param name="username">String: Username</param>
        /// <param name="group">String: Group name to check</param>
        /// <param name="domain">String: Domain</param>
        /// <returns>true iff the user is a member of group, false otherwise</returns>
        private bool isMemberOfGroup(string username, string group, string domain) {
            // AD Access is only available if AD is on the local machine
            if (OperatingSystem.IsWindows()) {
                // set up domain context
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, domain);

                // find a user
                UserPrincipal user = UserPrincipal.FindByIdentity(ctx, username);

                // find the group in question
                GroupPrincipal pGroup = GroupPrincipal.FindByIdentity(ctx, group);

                if (user != null) {
                   // check if user is member of that group
                   return user.IsMemberOf(pGroup);
                }
            }
            return false;
        }
    }
}