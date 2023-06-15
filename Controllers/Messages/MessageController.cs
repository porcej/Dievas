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
    ///     Controller Class <c>MessagesController</c> Provids a CRUD API to 
    ///     manage application messages
    ///
    ///   URI                   | Verb              | Outcome
    ///   =====================================================================
    ///   ...api/messages       | GET               | Lists messages
    ///   ...api/messages       | POST              | Create a new message
    ///   ...api/messages/id    | GET               | Get messages, id
    ///   ...api/messages/id    | PUT               | Update messages, id
    ///   ...api/messages/id    | DELETE            | Remove messages, id
    ///     
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize(Roles = "admin")]    // For admin Only
    public class MessagesController : ControllerBase {

        /// <summary>
        ///     Logging Controller for this class
        /// </summary>
        private readonly ILogger<MessagesController> _logger;

        /// <summary>
        ///     Database Access Object
        /// </summary>
        private AppDbContext _appDbContext;

        /// <summary>
        ///     Default constructor for Class <c>MessagesController</c>
        /// </summary>
        /// <param name="logger">ILogger: aggregate logger</param>
        /// <param name="appDbContext">AppDbContext: Database aacess</param>
        public MessagesController(ILogger<MessagesController> logger, AppDbContext appDbContext) {
            _logger = logger;
            _appDbContext = appDbContext;
        }

        /// <summary>
        ///   Listen for rquests to list all message types
        /// 
        ///   URI                   | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/messages       | GET               | Lists messages
        ///   
        /// </summary>
        /// <returns> Returns all messages (JSON) </returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetMessages() {
            var msgs = _appDbContext.Messages.Include(m => m.MessageType)
                                             .Include(m => m.Campaigns ).AsNoTracking();
            return Ok(await msgs.ToListAsync());
        }

        /// <summary>
        ///   Listen for rquests to add a new message type
        /// 
        ///   URI                   | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/messages       | POST              | Create a new messageType
        ///   
        /// </summary>
        /// <returns> Returns success message or error code </returns>
        [Authorize(Roles = "admin")]    // For admin Only
        [HttpPost]
        public async Task<ActionResult> CreateMessage(Message message) {
            try {
                if (message == null)
                    return BadRequest();
                _appDbContext.Messages.Add(message);
                await _appDbContext.SaveChangesAsync();
                return Ok(new { message = $"Message { message.Title } created successfully." });

            } catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error creating new message { message.Title }.");
            }
        }

        /// <summary>
        ///   Listen for requests for a single message type
        /// 
        ///   URI                   | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/messages/id    | GET               | Get messages, id
        ///   
        /// </summary>
        /// <param name="id">Id for the message being requested</param>
        /// <returns> Returns single message information.</returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetMessage(int id) {
            var msgs = _appDbContext.Messages.Include(m => m.MessageType)
                                             .Include(m => m.Campaigns ).AsNoTracking();
            return Ok(await msgs.FirstOrDefaultAsync(m => m.MessageId == id));
        }

        /// <summary>
        ///   Listen for requests to update single message
        /// 
        ///   URI                   | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/messages/id    | PUT               | Update messages, id
        ///   
        /// </summary>
        /// <param name="id">int: id for message whos data is being udpated</param>
        /// <param name="message">Updated messages information.</param>
        /// <returns> Returns the single message updated information.</returns>
        [Authorize(Roles = "admin")]    // For admin Only
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMessage(int id, Message message) {

            // Check if message exists
            Message msg = await _appDbContext.Messages.FindAsync(id);
            // Update message if it exits
            if (msg == null) {
                 return NotFound($"Message with ID# {id} not found.");
            }
            msg.copy(message);
            await _appDbContext.SaveChangesAsync();
            return Ok(msg);
        }

        /// <summary>
        ///   Listen for requests to remove a single message
        /// 
        ///   URI                   | Verb              | Outcome
        ///   =====================================================================
        ///   ...api/messages/id    | DELETE            | Remove messages, id
        ///   
        /// </summary>
        /// <param name="id">int: ID for message to be removed</param>
        /// <returns> Returns success message on messages removal.</returns>
        [Authorize(Roles = "admin")]    // For admin Only
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id) {
            // Check if message exists
            Message msg = await _appDbContext.Messages.FindAsync(id);
            // Update message if it exits
            if (msg == null) {
                 return NotFound($"Message with ID# {id} not found.");
            }
            string title = msg.Title;
            try {
                _appDbContext.Messages.Remove(msg);
                await _appDbContext.SaveChangesAsync();
                return Ok(new { message = $"Message {title} with ID { id } deleted successfully." });
            } catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error deleting message {title} with { id }.");
            }
        }
    }
}