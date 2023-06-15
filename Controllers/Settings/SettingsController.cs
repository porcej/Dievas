using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Dievas.Data;
using Dievas.Models.Settings;

namespace Dievas.Controllers {

    /// <summary>
    ///     Controller Class <c>SettingsController</c> Provids a CRUD API to 
    ///     manage application settings
    ///
    ///   URI                   | Verb              | Outcome
    ///   =====================================================================
    ///   ...api/settings       | GET               | Lists settings
    ///   ...api/settings       | POST              | Create a new setting
    ///   ...api/settings/id    | GET               | Get settings, id
    ///   ...api/settings/id    | PUT               | Update settings, id
    ///   ...api/settings/id    | DELETE            | Remove settings, id
    ///     
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize(Roles = "admin")]    // For admin Only
    public class SettingsController : ControllerBase {

        /// <summary>
        ///     Logging Controller for this class
        /// </summary>
        private readonly ILogger<SettingsController> _logger;

        /// <summary>
        ///     Database Access Object
        /// </summary>
        private AppDbContext _appDbContext;

        /// <summary>
        ///     Default constructor for Class <c>SettingsController</c>
        /// </summary>
        /// <param name="logger">ILogger: aggregate logger</param>
        /// <param name="appDbContext">AppDbContext: Database aacess</param>
        public SettingsController(ILogger<SettingsController> logger, AppDbContext appDbContext) {
            _logger = logger;
            _appDbContext = appDbContext;
        }

        /// <summary>
        ///   Listen for rquests to list all settings
        /// 
        ///   URI                   | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/settings       | GET               | Lists settings
        ///   
        /// </summary>
        /// <returns> Returns all settings (JSON) </returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetSettings() {
            var settings = _appDbContext.SystemSettings.Include(s => s.SettingType).AsNoTracking();
            return Ok(await settings.ToListAsync());
        }

        /// <summary>
        ///   Listen for rquests to add a new setting
        /// 
        ///   URI                   | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/settings       | POST              | Create a new setting
        ///   
        /// </summary>
        /// <returns> Returns success message or error code </returns>
        [Authorize(Roles = "admin")]    // For admin Only
        [HttpPost]
        public async Task<ActionResult> CreateSetting(SystemSetting setting) {
            try {
                if (setting == null)
                    return BadRequest();
                _appDbContext.SystemSettings.Add(setting);
                await _appDbContext.SaveChangesAsync();
                return Ok(new { message = $"Setting { setting.Field } created successfully." });

            } catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error creating new setting { setting.Field }.");
            }
        }

        /// <summary>
        ///   Listen for requests for a single setting
        /// 
        ///   URI                   | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/settings/id    | GET               | Get settings, id
        ///   
        /// </summary>
        /// <param name="id">Id for the setting being requested</param>
        /// <returns> Returns single setting information.</returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetSetting(int id) {
            var setting = _appDbContext.SystemSettings.Include(s => s.SettingType).AsNoTracking();
            return Ok(await setting.FirstOrDefaultAsync(s => s.SystemSettingId == id));
        }

        /// <summary>
        ///   Listen for requests to update single setting
        /// 
        ///   URI                   | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/settings/id    | PUT               | Update settings, id
        ///   
        /// </summary>
        /// <param name="id">int: id for setting whos data is being udpated</param>
        /// <param name="setting">Updated settings information.</param>
        /// <returns> Returns the single setting updated information.</returns>
        [Authorize(Roles = "admin")]    // For admin Only
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSetting(int id, SystemSetting setting) {

            // Check if setting exists
            SystemSetting ss = await _appDbContext.SystemSettings.FindAsync(id);
            // Update setting if it exits
            if (ss == null) {
                 return NotFound($"Setting with ID# {id} not found.");
            }
            ss.copy(setting);
            await _appDbContext.SaveChangesAsync();
            return Ok(ss);
        }

        /// <summary>
        ///   Listen for requests to remove a single setting
        /// 
        ///   URI                   | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/settings/id    | DELETE            | Remove settings, id
        ///   
        /// </summary>
        /// <param name="id">int: ID for setting to be removed</param>
        /// <returns> Returns success message on settings removal.</returns>
        [Authorize(Roles = "admin")]    // For admin Only
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSetting(int id) {
            // Check if setting exists
            SystemSetting ss = await _appDbContext.SystemSettings.FindAsync(id);
            // Update setting if it exits
            if (ss == null) {
                 return NotFound($"Setting with ID# {id} not found.");
            }
            string field = ss.Field;
            try {
                _appDbContext.SystemSettings.Remove(ss);
                await _appDbContext.SaveChangesAsync();
                return Ok(new { message = $"Setting {field} ID# { id } deleted successfully." });
            } catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error deleting setting {field} ID# { id }.");
            }
        }
    }
}