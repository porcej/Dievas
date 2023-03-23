using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Dievas.Models.Auth;

namespace Dievas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        //For admin Only
        [HttpGet]
        [Route("Admins")]
        [Authorize(Roles = "admin")]
        public IActionResult AdminEndPoint()
        {
            UserModel currentUser = GetCurrentUser();
            // return Ok($"Hi you are {string.Join(", ", currentUser.Roles)}.");
            return Ok($"Hi you are {currentUser.Username} with the following rolls: {string.Join(", ", currentUser.Roles)}.");

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