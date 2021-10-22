using BlazzingChat.Server.Data;
using BlazzingChat.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazzingChat.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly BlazzingChatDbContext _context;
        private readonly ILogger<UsersController> _logger;

        public UsersController(BlazzingChatDbContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetUsers()
        {
            List<Contact> contacts = await _context.Users
                .Select(u => new Contact()
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                })
                .ToListAsync();

            if (contacts is null)
            {
                return NotFound();
            }

            return contacts;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            var userToUpdate = await _context.Users.FindAsync(id);

            if (userToUpdate is null)
            {
                return NotFound();
            }

            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.EmailAddress = user.EmailAddress;

            _context.Users.Update(userToUpdate);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return NoContent();
        }

        // PUT: api/Users/UpdateSettings/5
        [HttpPut("UpdateSettings/{id}")]
        public async Task<IActionResult> UpdateSettings(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            var userToUpdate = await _context.Users.FindAsync(id);

            if (userToUpdate is null)
            {
                return NotFound();
            }

            userToUpdate.Notifications = user.Notifications;
            userToUpdate.DarkTheme = user.DarkTheme;

            _context.Users.Update(userToUpdate);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        //Authentication methods
        // POST: api/Users/loginuser
        [HttpPost("loginuser")]
        public async Task<ActionResult<User>> LoginUser(User user)
        {
            User loggedInUser = await _context.Users
                .FirstOrDefaultAsync(u => u.EmailAddress.Equals(user.EmailAddress) &&
                                          u.Password.Equals(user.Password));
            if (user is not null)
            {
                //create a claim
                Claim claim = new Claim(ClaimTypes.Name, loggedInUser.EmailAddress);
                //create a claimsIdentity
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] { claim }, "serverAuth");
                //create a claimsPrincipal
                ClaimsPrincipal claimsPrincipal = new(claimsIdentity);
                //Sign in user
                await HttpContext.SignInAsync(claimsPrincipal);
            }

            return await Task.FromResult(loggedInUser);
        }

        // GET: api/Users/getcurrentuser
        [HttpGet("getcurrentuser")]
        public async Task<ActionResult<User>> GetCurrentUser()
        {
            User currentUser = new User();

            if (User.Identity.IsAuthenticated)
            {
                currentUser.EmailAddress = User.FindFirstValue(ClaimTypes.Name);
            }

            return await Task.FromResult(currentUser);
        }

        [HttpGet("logoutuser")]
        // GET: api/Users/logoutuser
        public async Task<ActionResult<String>> LogoutUser()
        {
            await HttpContext.SignOutAsync();
            return "Success";
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
