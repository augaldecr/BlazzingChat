using BlazzingChat.Server.Data;
using BlazzingChat.Server.Helpers;
using BlazzingChat.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazzingChat.Server.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
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
        [Authorize]
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
        [Authorize]
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
            userToUpdate.AboutMe = user.AboutMe;
            if (user.ProfilePictDataUrl is not null)
            {
                userToUpdate.ProfilePictDataUrl = user.ProfilePictDataUrl;
            }

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
        [Authorize]
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
        [Authorize]
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
            //user.Password = Utilities.Encrypt(user.Password);
            User loggedInUser = await _context.Users
                .FirstOrDefaultAsync(u => u.EmailAddress.Equals(user.EmailAddress) &&
                                          u.Password.Equals(user.Password));
            if (loggedInUser is not null)
            {
                //create a claim
                Claim claimEmail = new(ClaimTypes.Email, loggedInUser.EmailAddress);
                Claim claimIdentifier = new(ClaimTypes.NameIdentifier, loggedInUser.Id.ToString());
                //create a claimsIdentity
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] { claimEmail, claimIdentifier }, "serverAuth");
                //create a claimsPrincipal
                ClaimsPrincipal claimsPrincipal = new(claimsIdentity);

                var authProperties = GetAuthenticationProperties;

                //Sign in user
                try
                {
                    await HttpContext.SignInAsync(claimsPrincipal, authProperties);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message); ;
                }
            }

            return Ok(loggedInUser);
        }

        // GET: api/Users/getcurrentuser
        [HttpGet("getcurrentuser")]
        public async Task<ActionResult<User>> GetCurrentUser()
        {
            User currentUser = new();

            if (User.Identity.IsAuthenticated)
            {
                currentUser = await _context.Users
                                    .FirstOrDefaultAsync(u => u.EmailAddress.Equals(User.FindFirstValue(ClaimTypes.Email)));
                if (currentUser is null)
                {
                    currentUser = new();
                    currentUser.Id = _context.Users.Max(u => u.Id) + 1;
                    currentUser.EmailAddress = User.FindFirstValue(ClaimTypes.Email);
                    currentUser.Password = Utilities.Encrypt(currentUser.EmailAddress);
                    currentUser.Source = "EXTL";

                    _context.Users.Add(currentUser);
                    await _context.SaveChangesAsync();
                }
            }

            return Ok(currentUser);
        }

        // GET: api/Users/getonlyvisiblecontacts
        [Authorize]
        [HttpGet("getonlyvisiblecontacts")]
        public ActionResult<List<User>> GetOnlyVisibleContacts(int startIndex, int count)
        {
            List<User> users = new();
            users.AddRange(Enumerable.Range(startIndex, count).Select(x => new User 
                                                            { Id = x, FirstName = $"First{x}", LastName = $"Last{x}" }));

            return users;
        }

        // GET: api/Users/getvisiblecontacts
        [Authorize]
        [HttpGet("getvisiblecontacts")]
        public async Task<List<User>> GetVisibleContacts(int startIndex, int count) =>
            await _context.Users.Skip(startIndex).Take(count).ToListAsync();

        // GET: api/Users/logoutuser
        [Authorize]
        [HttpGet("logoutuser")]
        public async Task<ActionResult<String>> LogoutUser()
        {
            await HttpContext.SignOutAsync();
            return "Success";
        }

        // GET: api/Users/getcontactscount
        [Authorize]
        [HttpGet("getcontactscount")]
        public async Task<int> GetContactsCount()
        {
            throw new IndexOutOfRangeException();
            await _context.Users.CountAsync();
        }

        // GET: api/Users/TwitterSignIn
        [HttpGet("TwitterSignIn")]
        public async Task TwitterSignIn() =>  await HttpContext
            .ChallengeAsync(TwitterDefaults.AuthenticationScheme, GetAuthenticationProperties);

        // GET: api/Users/FacebookSignIn
        [HttpGet("FacebookSignIn")]
        public async Task FacebookSignIn() => await HttpContext
            .ChallengeAsync(FacebookDefaults.AuthenticationScheme, GetAuthenticationProperties);

        // GET: api/Users/GoogleSignIn
        [HttpGet("GoogleSignIn")]
        public async Task GoogleSignIn() => await HttpContext
            .ChallengeAsync(GoogleDefaults.AuthenticationScheme, GetAuthenticationProperties);

        // GET: api/Users/DownloadServerFile
        [Authorize]
        [HttpGet("DownloadServerFile")]
        public async Task<ActionResult<string>> DownloadServerFile()
        {
            var filePath = "idl_estrategias.docx";

            try
            {
                using var fileInput = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                MemoryStream memoryStream = new MemoryStream();
                await fileInput.CopyToAsync(memoryStream);

                var buffer = memoryStream.ToArray();
                return Convert.ToBase64String(buffer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }

        }

        // DELETE: api/Users/5
        [Authorize]
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

        public AuthenticationProperties GetAuthenticationProperties => new()
                                                                            {
                                                                                IsPersistent = true,
                                                                                ExpiresUtc = DateTime.Now.AddMinutes(30),
                                                                                RedirectUri = "/profile",
                                                                            };

        [HttpGet("notauthorized")]
        public IActionResult NotAuthorized()
        {
            return Unauthorized();
        }
    }
}
