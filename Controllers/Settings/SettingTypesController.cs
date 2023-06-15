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
    ///     Controller Class <c>SettingTypesController</c> Provids a CRUD API to 
    ///     manage application settingTypes
    ///
    ///   URI                       | Verb              | Outcome
    ///   =====================================================================
    ///   ...api/settingTypes       | GET               | Lists settingTypes
    ///   ...api/settingTypes       | POST              | Create a new setting
    ///   ...api/settingTypes/id    | GET               | Get settingTypes, id
    ///   ...api/settingTypes/id    | PUT               | Update settingTypes, id
    ///   ...api/settingTypes/id    | DELETE            | Remove settingTypes, id
    ///     
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize(Roles = "admin")]    // For admin Only
    public class SettingTypesController : ControllerBase {

        /// <summary>
        ///     Logging Controller for this class
        /// </summary>
        private readonly ILogger<SettingTypesController> _logger;

        /// <summary>
        ///     Database Access Object
        /// </summary>
        private AppDbContext _appDbContext;

        /// <summary>
        ///     Default constructor for Class <c>SettingTypesController</c>
        /// </summary>
        /// <param name="logger">ILogger: aggregate logger</param>
        /// <param name="appDbContext">AppDbContext: Database aacess</param>
        public SettingTypesController(ILogger<SettingTypesController> logger, AppDbContext appDbContext) {
            _logger = logger;
            _appDbContext = appDbContext;
        }

        /// <summary>
        ///   Listen for rquests to list all settingTypes
        /// 
        ///   URI                       | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/settingTypes       | GET               | Lists settingTypes
        ///   
        /// </summary>
        /// <returns> Returns all settingTypes (JSON) </returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetSettingTypes() {
            var sts = _appDbContext.SettingTypes.AsNoTracking();
            return Ok(await sts.ToListAsync());
        }

        /// <summary>
        ///   Listen for rquests to add a new setting
        /// 
        ///   URI                       | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/settingTypes       | POST              | Create a new setting
        ///   
        /// </summary>
        /// <returns> Returns success message or error code </returns>
        [Authorize(Roles = "admin")]    // For admin Only
        [HttpPost]
        public async Task<ActionResult> CreateSetting(SettingType setting) {
            try {
                if (setting == null)
                    return BadRequest();
                _appDbContext.SettingTypes.Add(setting);
                await _appDbContext.SaveChangesAsync();
                return Ok(new { message = $"Setting { setting.Name } created successfully." });

            } catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error creating new setting { setting.Name }.");
            }
        }

        /// <summary>
        ///   Listen for requests for a single setting
        /// 
        ///   URI                       | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/settingTypes/id    | GET               | Get settingTypes, id
        ///   
        /// </summary>
        /// <param name="id">Id for the setting being requested</param>
        /// <returns> Returns single setting information.</returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetSetting(int id) {
            var sts = _appDbContext.SettingTypes.AsNoTracking();
            return Ok(await sts.FirstOrDefaultAsync(s => s.SettingTypeId == id));
        }

        /// <summary>
        ///   Listen for requests to update single setting
        /// 
        ///   URI                       | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/settingTypes/id    | PUT               | Update settingTypes, id
        ///   
        /// </summary>
        /// <param name="id">int: id for setting whos data is being udpated</param>
        /// <param name="setting">Updated settingTypes information.</param>
        /// <returns> Returns the single setting updated information.</returns>
        [Authorize(Roles = "admin")]    // For admin Only
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSetting(int id, SettingType settingType) {

            // Check if setting exists
            SettingType st = await _appDbContext.SettingTypes.FindAsync(id);
            // Update setting if it exits
            if (st == null) {
                 return NotFound($"Setting with ID# {id} not found.");
            }
            st.copy(settingType);
            await _appDbContext.SaveChangesAsync();
            return Ok(st);
        }

        /// <summary>
        ///   Listen for requests to remove a single setting
        /// 
        ///   URI                       | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/settingTypes/id    | DELETE            | Remove settingTypes, id
        ///   
        /// </summary>
        /// <param name="id">int: ID for setting to be removed</param>
        /// <returns> Returns success message on settingTypes removal.</returns>
        [Authorize(Roles = "admin")]    // For admin Only
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSetting(int id) {
            // Check if setting exists
            SettingType st = await _appDbContext.SettingTypes.FindAsync(id);
            // Update setting if it exits
            if (st == null) {
                 return NotFound($"Setting with ID# {id} not found.");
            }
            try {
                _appDbContext.SettingTypes.Remove(st);
                await _appDbContext.SaveChangesAsync();
                return Ok(new { message = $"Setting { id } deleted successfully." });
            } catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error deleting setting { id }.");
            }
        }
    }
}