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
    ///     Controller Class <c>MessageTypesController</c> Provids a CRUD API to 
    ///     manage application message types
    ///
    ///   URI                       | Verb              | Outcome
    ///   =====================================================================
    ///   ...api/messageTypes       | GET               | Lists messageTypes
    ///   ...api/messageTypes       | POST              | Create a new message
    ///   ...api/messageTypes/id    | GET               | Get messageTypes, id
    ///   ...api/messageTypes/id    | PUT               | Update messageTypes, id
    ///   ...api/messageTypes/id    | DELETE            | Remove messageTypes, id
    ///     
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize(Roles = "admin")]    // For admin Only
    public class MessageTypesController : ControllerBase {

        /// <summary>
        ///     Logging Controller for this class
        /// </summary>
        private readonly ILogger<MessageTypesController> _logger;

        /// <summary>
        ///     Database Access Object
        /// </summary>
        private AppDbContext _appDbContext;

        /// <summary>
        ///     Default constructor for Class <c>MessageTypesController</c>
        /// </summary>
        /// <param name="logger">ILogger: aggregate logger</param>
        /// <param name="appDbContext">AppDbContext: Database aacess</param>
        public MessageTypesController(ILogger<MessageTypesController> logger, AppDbContext appDbContext) {
            _logger = logger;
            _appDbContext = appDbContext;
        }

        /// <summary>
        ///   Listen for rquests to list all message types
        /// 
        ///   URI                       | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/messageTypes       | GET               | Lists messageTypes
        ///   
        /// </summary>
        /// <returns> Returns all messageTypes (JSON) </returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetMessageTypes() {
            var msgs = _appDbContext.MessageTypes.AsNoTracking();
            return Ok(await msgs.ToListAsync());
        }

        /// <summary>
        ///   Listen for rquests to add a new message type
        /// 
        ///   URI                       | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/messageTypes       | POST              | Create a new messageType
        ///   
        /// </summary>
        /// <returns> Returns success message or error code </returns>
        [Authorize(Roles = "admin")]    // For admin Only
        [HttpPost]
        public async Task<ActionResult> CreateMessage(MessageType message) {
            try {
                if (message == null)
                    return BadRequest();
                _appDbContext.MessageTypes.Add(message);
                await _appDbContext.SaveChangesAsync();
                return Ok(new { message = $"Message { message.Name } created successfully." });

            } catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error creating new message { message.Name }.");
            }
        }

        /// <summary>
        ///   Listen for requests for a single message type
        /// 
        ///   URI                       | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/messageTypes/id    | GET               | Get messageTypes, id
        ///   
        /// </summary>
        /// <param name="id">Id for the message being requested</param>
        /// <returns> Returns single message information.</returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetMessage(int id) {
            return Ok(await _appDbContext.MessageTypes.FindAsync(id));
        }

        /// <summary>
        ///   Listen for requests to update single message
        /// 
        ///   URI                       | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/messageTypes/id    | PUT               | Update messageTypes, id
        ///   
        /// </summary>
        /// <param name="id">int: id for message whos data is being udpated</param>
        /// <param name="message">Updated messageTypes information.</param>
        /// <returns> Returns the single message updated information.</returns>
        [Authorize(Roles = "admin")]    // For admin Only
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMessage(int id, MessageType messageType) {

            // Check if message exists
            MessageType msg = await _appDbContext.MessageTypes.FindAsync(id);
            // Update message if it exits
            if (msg == null) {
                 return NotFound($"Message with ID# {id} not found.");
            }
            msg.copy(messageType);
            await _appDbContext.SaveChangesAsync();
            return Ok(msg);
        }

        /// <summary>
        ///   Listen for requests to remove a single message
        /// 
        ///   URI                       | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/messageTypes/id    | DELETE            | Remove messageTypes, id
        ///   
        /// </summary>
        /// <param name="id">int: ID for message to be removed</param>
        /// <returns> Returns success message on messageTypes removal.</returns>
        [Authorize(Roles = "admin")]    // For admin Only
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id) {
            // Check if message exists
            MessageType msg = await _appDbContext.MessageTypes.FindAsync(id);
            // Update message if it exits
            if (msg == null) {
                 return NotFound($"Message with ID# {id} not found.");
            }
            try {
                _appDbContext.MessageTypes.Remove(msg);
                await _appDbContext.SaveChangesAsync();
                return Ok(new { message = $"Message { id } deleted successfully." });
            } catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error deleting message { id }.");
            }
        }
    }
}