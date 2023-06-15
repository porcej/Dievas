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
using Dievas.Models.Messages;

namespace Dievas.Controllers {

    /// <summary>
    ///     Controller Class <c>CampaignsController</c> Provids a CRUD API to 
    ///     manage application campaign campaigns
    ///
    ///   URI                    | Verb              | Outcome
    ///   =====================================================================
    ///   ...api/campaigns       | GET               | Lists campaigns
    ///   ...api/campaigns       | POST              | Create a new campaign
    ///   ...api/campaigns/id    | GET               | Get campaigns, id
    ///   ...api/campaigns/id    | PUT               | Update campaigns, id
    ///   ...api/campaigns/id    | DELETE            | Remove campaigns, id
    ///     
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize(Roles = "admin")]    // For admin Only
    public class CampaignsController : ControllerBase {

        /// <summary>
        ///     Logging Controller for this class
        /// </summary>
        private readonly ILogger<CampaignsController> _logger;

        /// <summary>
        ///     Database Access Object
        /// </summary>
        private AppDbContext _appDbContext;

        /// <summary>
        ///     Default constructor for Class <c>CampaignsController</c>
        /// </summary>
        /// <param name="logger">ILogger: aggregate logger</param>
        /// <param name="appDbContext">AppDbContext: Database aacess</param>
        public CampaignsController(ILogger<CampaignsController> logger, AppDbContext appDbContext) {
            _logger = logger;
            _appDbContext = appDbContext;
        }

        /// <summary>
        ///   Listen for rquests to list all campaign types
        /// 
        ///   URI                    | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/campaigns       | GET               | Lists campaigns
        ///   
        /// </summary>
        /// <returns> Returns all campaigns (JSON) </returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetCampaigns() {
            var msgs = _appDbContext.Campaigns.Include(c => c.Messages).AsNoTracking();
            return Ok(await msgs.ToListAsync());
        }

        /// <summary>
        ///   Listen for rquests to add a new campaign type
        /// 
        ///   URI                    | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/campaigns       | POST              | Create a new campaignType
        ///   
        /// </summary>
        /// <returns> Returns success campaign or error code </returns>
        [Authorize(Roles = "admin")]    // For admin Only
        [HttpPost]
        public async Task<ActionResult> CreateCampaign(Campaign campaign) {
            try {
                if (campaign == null)
                    return BadRequest();
                _appDbContext.Campaigns.Add(campaign);
                await _appDbContext.SaveChangesAsync();
                return Ok(new { campaign = $"Campaign { campaign.Title } created successfully." });

            } catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error creating new campaign { campaign.Title }.");
            }
        }

        /// <summary>
        ///   Listen for requests for a single campaign type
        /// 
        ///   URI                    | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/campaigns/id    | GET               | Get campaigns, id
        ///   
        /// </summary>
        /// <param name="id">Id for the campaign being requested</param>
        /// <returns> Returns single campaign information.</returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCampaign(int id) {
            var campaigns = _appDbContext.Campaigns.Include(c => c.Messages).AsNoTracking();
            return Ok(await campaigns.FirstOrDefaultAsync(c => c.CampaignId == id));
        }

        /// <summary>
        ///   Listen for requests to update single campaign
        /// 
        ///   URI                    | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/campaigns/id    | PUT               | Update campaigns, id
        ///   
        /// </summary>
        /// <param name="id">int: id for campaign whos data is being udpated</param>
        /// <param name="campaign">Updated campaigns information.</param>
        /// <returns> Returns the single campaign updated information.</returns>
        [Authorize(Roles = "admin")]    // For admin Only
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCampaign(int id, Campaign campaign) {

            // Check if campaign exists
            Campaign camp = await _appDbContext.Campaigns.FindAsync(id);
            // Update campaign if it exits
            if (camp == null) {
                 return NotFound($"Campaign with ID# {id} not found.");
            }
            camp.copy(campaign);
            await _appDbContext.SaveChangesAsync();
            return Ok(camp);
        }

        /// <summary>
        ///   Listen for requests to remove a single campaign
        /// 
        ///   URI                    | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/campaigns/id    | DELETE            | Remove campaigns, id
        ///   
        /// </summary>
        /// <param name="id">int: ID for campaign to be removed</param>
        /// <returns> Returns success campaign on campaigns removal.</returns>
        [Authorize(Roles = "admin")]    // For admin Only
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCampaign(int id) {
            // Check if campaign exists
            Campaign campaign = await _appDbContext.Campaigns.FindAsync(id);
            // Update campaign if it exits
            if (campaign == null) {
                 return NotFound($"Campaign with ID# {id} not found.");
            }
            string title = campaign.Title;
            try {
                _appDbContext.Campaigns.Remove(campaign);
                await _appDbContext.SaveChangesAsync();
                return Ok(new { campaign = $"Campaign {title} with ID { id } deleted successfully." });
            } catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error deleting campaign {title} with { id }.");
            }
        }
    }
}