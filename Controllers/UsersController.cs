using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Dievas.Models.Auth;

namespace Dievas.Controllers {

    /// <summary>
    ///     Controller Class <c>UsersController</c> Provids a CRUD API to 
    ///     manage user roles
    /// 
    ///   URI                   | Verb              | Outcome
    ///   =====================================================================
    ///   ...api/users          | GET               | Lists users
    ///   ...api/users          | POST              | Create a new user
    ///   ...api/users/username | GET               | Get user, username
    ///   ...api/users/username | PUT               | Update user, username
    ///   ...api/users/username | DELETE            | Remove user, username
    ///     
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]    // For admin Only
    public class UsersController : ControllerBase {

        /// <summary>
        ///     Logging Controller for this class
        /// </summary>
        private readonly ILogger<UsersController> _logger;

        /// <summary>
        ///     Default constructor for Class <c>UsersController</c>
        /// </summary>
        /// <param name="logger">ILogger: aggregate logger</param>
        public UsersController(ILogger<UsersController> logger) {
            _logger = logger;
        }

        /// <summary>
        ///   Listen for rquests to list all users
        /// 
        ///   URI                   | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/users          | GET               | Lists users
        ///   
        /// </summary>
        /// <param name="role"> Optional, if specified only returns users with the
        ///   the provided role.  Role is passed via query string
        /// </param>
        /// <returns> Returns all users (JSON) </returns>
        // [AllowAnonymous]
        [HttpGet]
        // public IActionResult GetAllUsers() {
        public async Task<ActionResult> GetUsers(string? role) {
            if (role == null)
                return Ok(Access.Users);
            return Ok(Access.Users.Where(u => u.Roles.Contains(role)));
        }

        /// <summary>
        ///   Listen for rquests to add a new user
        /// 
        ///   URI                   | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/users          | POST              | Create a new user
        ///   
        /// </summary>
        /// <returns> Returns success message or error cde </returns>
        // [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> CreateUser(UserModel user) {
            try {
                if (user == null)
                    return BadRequest();
                Access.Users.Add(user);
                return Ok(new { message = $"User { user.Username } created successfully." });

            } catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new user");
            }
        }

        /// <summary>
        ///   Listen for requests for a single user
        /// 
        ///   URI                   | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/users/username | GET               | Get user, username
        ///   
        /// </summary>
        /// <param name="username">Username for user information being requested</param>
        /// <returns> Returns single user information.</returns>
        [HttpGet("{username}")]
        public async Task<ActionResult> GetUser(string username) {
            return Ok(Access.Users.FirstOrDefault(u => u.Username == username));
        }

        /// <summary>
        ///   Listen for requests to update single user
        /// 
        ///   URI                   | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/users/username | PUT               | Update user, username
        ///   
        /// </summary>
        /// <param name="username">Username for user whos data is being udpated</param>
        /// <param name="model">Updated model information.</param>
        /// <returns> Returns single user updated information.</returns>
        [HttpPut("{username}")]
        public async Task<ActionResult> UpdateUser(string username, UserModel model) {

            // Check if user exists
            int udx = Access.Users.FindIndex(u => u.Username == username);
            // Update user if it exits
            if (udx == -1) {
                 return NotFound($"User {username} not found.");
            }
            Access.Users[udx] = model;
            return Ok(Access.Users[udx]);
        }

        /// <summary>
        ///   Listen for requests to remove a single user
        /// 
        ///   URI                   | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/users/username | DELETE            | Remove user, username
        ///   
        /// </summary>
        /// <param name="username">Username for user to be removed</param>
        /// <returns> Returns success message on user delete.</returns>
        [HttpDelete("{username}")]
        public async Task<ActionResult> DeleteUser(string username) {
            // Check if user exists
            int udx = Access.Users.FindIndex(u => u.Username == username);
            // Update user if it exits
            if (udx == -1) {
                 return NotFound($"User {username} not found.");
            }
            try {
                Access.Users.RemoveAt(udx);
                return Ok(new { message = $"User { username } deleted successfully." });
            } catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting user.");
            }
        }
    }
}