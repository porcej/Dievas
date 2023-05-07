using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Dievas.Models.Auth;

namespace Dievas.Controllers {

    /// <summary>
    ///     Controller Class <c>AccessController</c> Provids a CRUD API to 
    ///     manage user roles
    ///     
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]    // For admin Only
    public class AccessController : ControllerBase {

        /// <summary>
        ///     Logging Controller for this class
        /// </summary>
        private readonly ILogger<AccessController> _logger;

        /// <summary>
        ///     Default constructor for Class <c>AccessController</c>
        /// </summary>
        /// <param name="logger">ILogger: aggregate logger</param>
        public AccessController(ILogger<AccessController> logger) {
            _logger = logger;
        }

        /// <summary>
        ///     Listen for rquests to list all users
        /// </summary>
        /// <returns> Returns all users </returns>
        // [AllowAnonymous]
        [HttpGet("users")]
        public IActionResult GetAllUsers() {
            return Ok(Access.Users);
        }

        /// <summary>
        ///     Listen for requests for a single user
        /// </summary>
        /// <returns> Returns all users </returns>
        [HttpGet("user/{username}")]
        public IActionResult GetUser(string username) {
            return Ok(Access.Users.FirstOrDefault(u => u.Username == username));
        }

        // [AllowAnonymous]
        [HttpPost("user/addorupdate/")]
        public IActionResult AddOrUpdateUser(UserModel model){

            // Check if user exists
            int udx = Access.Users.FindIndex(u => u.Username == model.Username);
            // Update user if it exits
            if (udx != -1) {
                Access.Users[udx] = model;
                return Ok(new { message = $"User { model.Username } updated successfully." });
            }

            // Add new user if user does not exist
            Access.Users.Add(model);
            return Ok(new { message = $"User { model.Username } added successfully." });
        }

        /// <summary>
        ///     Listen for rquests to list all roles
        /// </summary>
        /// <returns> Returns all roles </returns>
        [HttpGet("roles")]
        public IActionResult GetAllRoles() {
            return Ok(Access.Roles);
        }

        /// <summary>
        ///     Listen for requests for a single role
        /// </summary>
        /// <returns> Returns all roles </returns>
        [HttpGet]
        [Route("role/{role}")]
        public IActionResult GetRole(string role) {
            return Ok(Access.Roles.FirstOrDefault(r => r == role));
        }

        private UserModel GetCurrentUser() {

            Console.WriteLine("HERE HERE HERE HERE HERE HERE HERE HERE HERE HERE HERE HERE HERE HERE HERE HERE HERE HERE HERE HERE HERE HERE HERE HERE");
            ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                UserModel user = new UserModel(); 
                user.Username = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                user.Roles = userClaims.Where(x => x.Type == ClaimTypes.Role)?.Select(x=> x.Value).ToList();
                return user;
            }

            return null;
        }
    }
}