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
    ///     Controller Class <c>RolesController</c> Provids a CRUD API to 
    ///     manage application roles
    /// 
    ///   URI                   | Verb              | Outcome
    ///   =====================================================================
    ///   ...api/roles          | GET               | Lists roles
    ///   ...api/roles          | POST              | Create a new role
    ///   ...api/roles/role     | GET               | Get role, role
    ///   ...api/roles/role     | PUT               | Update role, role
    ///   ...api/roles/role     | DELETE            | Remove role, role
    ///     
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]    // For admin Only
    public class RolesController : ControllerBase {

        /// <summary>
        ///     Logging Controller for this class
        /// </summary>
        private readonly ILogger<RolesController> _logger;

        /// <summary>
        ///     Default constructor for Class <c>RolesController</c>
        /// </summary>
        /// <param name="logger">ILogger: aggregate logger</param>
        public RolesController(ILogger<RolesController> logger) {
            _logger = logger;
        }

        /// <summary>
        ///   Listen for rquests to list all roles
        /// 
        ///   URI                   | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/roles          | GET               | Lists roles
        ///   
        /// </summary>
        /// <returns> Returns all roles (JSON) </returns>
        [HttpGet]
        // public IActionResult GetAllUsers() {
        public async Task<ActionResult> GetRoles() {
            return Ok(Access.Users);
        }

        /// <summary>
        ///   Listen for rquests to add a new role
        /// 
        ///   URI                   | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/roles          | POST              | Create a new role
        ///   
        /// </summary>
        /// <returns> Returns success message or error code </returns>
        [HttpPost]
        public async Task<ActionResult> CreateRole(string role) {
            try {
                if (role == null)
                    return BadRequest();
                Access.Roles.Add(role);
                return Ok(new { message = $"Role { role } created successfully." });

            } catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new user");
            }
        }

        /// <summary>
        ///   Listen for requests for a single role
        /// 
        ///   URI                   | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/roles/role     | GET               | Get role, role
        ///   
        /// </summary>
        /// <param name="role">role to view</param>
        /// <returns> Returns single role information.</returns>
        [HttpGet("{role}")]
        public async Task<ActionResult> GetRole(string role) {
            return Ok(Access.Roles.FirstOrDefault(r => r == role));
        }

        /// <summary>
        ///   Listen for requests to update single role
        /// 
        ///   URI                   | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/roles/role | PUT               | Update role, role
        ///   
        /// </summary>
        /// <param name="role">role to update</param>
        /// <param name="new_role">updated role</param>
        /// <returns> Returns single user updated information.</returns>
        [HttpPut("{role}")]
        public async Task<ActionResult> UpdateRole(string role, string new_role) {

            // Check if user exists
            int rdx = Access.Roles.FindIndex(r => r == role);
            // Update user if it exits
            if (rdx == -1) {
                 return NotFound($"Role {role} not found.");
            }
            Access.Roles[rdx] = new_role;
            return Ok(Access.Roles[rdx]);
        }

        /// <summary>
        ///   Listen for requests to remove a single role
        /// 
        ///   URI                   | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/roles/role | DELETE            | Remove role, role
        ///   
        /// </summary>
        /// <param name="role">Role to be removed</param>
        /// <returns> Returns success message on role removal.</returns>
        [HttpDelete("{role}")]
        public async Task<ActionResult> DeleteRole(string role) {
            // Check if user exists
            int rdx = Access.Roles.FindIndex(r => r == role);
            // Update user if it exits
            if (rdx == -1) {
                 return NotFound($"Role {role} not found.");
            }
            try {
                Access.Roles.RemoveAt(rdx);
                return Ok(new { message = $"Role { role } deleted successfully." });
            } catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting role.");
            }
        }
    }
}