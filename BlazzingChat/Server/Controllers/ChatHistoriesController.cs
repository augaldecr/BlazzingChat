using BlazzingChat.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazzingChat.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatHistoriesController : ControllerBase
    {
        private readonly BlazzingChatDbContext _context;

        public ChatHistoriesController(BlazzingChatDbContext context)
        {
            _context = context;
        }

        // GET: api/ChatHistories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatHistory>>> GetChatHistories()
        {
            return await _context.ChatHistories.ToListAsync();
        }

        // GET: api/ChatHistories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChatHistory>> GetChatHistory(int id)
        {
            var chatHistory = await _context.ChatHistories.FindAsync(id);

            if (chatHistory == null)
            {
                return NotFound();
            }

            return chatHistory;
        }

        // PUT: api/ChatHistories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChatHistory(int id, ChatHistory chatHistory)
        {
            if (id != chatHistory.Id)
            {
                return BadRequest();
            }

            _context.Entry(chatHistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatHistoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ChatHistories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ChatHistory>> PostChatHistory(ChatHistory chatHistory)
        {
            _context.ChatHistories.Add(chatHistory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChatHistory", new { id = chatHistory.Id }, chatHistory);
        }

        // DELETE: api/ChatHistories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChatHistory(int id)
        {
            var chatHistory = await _context.ChatHistories.FindAsync(id);
            if (chatHistory == null)
            {
                return NotFound();
            }

            _context.ChatHistories.Remove(chatHistory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChatHistoryExists(int id)
        {
            return _context.ChatHistories.Any(e => e.Id == id);
        }
    }
}
