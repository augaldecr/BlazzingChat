using BlazzingChat.Server.Data;
using BlazzingChat.Server.Helpers;
using BlazzingChat.Shared;
using BlazzingChat.Shared.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
        private readonly IConfiguration _configuration;

        public UsersController(BlazzingChatDbContext context, 
                               ILogger<UsersController> logger,
                               IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
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
        public async Task<ActionResult<Data.User>> GetUser(int id)
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
        public async Task<IActionResult> PutUser(int id, Data.User user)
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
        public async Task<IActionResult> UpdateSettings(int id, Data.User user)
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
        public async Task<ActionResult<Data.User>> PostUser(Data.User user)
        {
            var emailAddressExists = _context.Users.FirstOrDefault(u => u.EmailAddress == user.EmailAddress);
            if (emailAddressExists is null)
            {
                //user.Password = Utility.Encrypt(user.Password);
                user.Password = user.Password;
                user.Source = "APPL";
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        //Authentication methods
        // POST: api/Users/loginuser
        [HttpPost("loginuser")]
        public async Task<ActionResult<Data.User>> LoginUser(Data.User user, bool isPersistent)
        {
            //user.Password = Utilities.Encrypt(user.Password);
            Data.User loggedInUser = await _context.Users
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

                //Sign in user
                try
                {
                    await HttpContext.SignInAsync(claimsPrincipal, GetAuthenticationProperties(isPersistent));
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
        public async Task<ActionResult<Data.User>> GetCurrentUser()
        {
            Data.User currentUser = new();

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
        public ActionResult<List<Data.User>> GetOnlyVisibleContacts(int startIndex, int count)
        {
            List<Data.User> users = new();
            users.AddRange(Enumerable.Range(startIndex, count).Select(x => new Data.User
            { Id = x, FirstName = $"First{x}", LastName = $"Last{x}" }));

            return users;
        }

        // GET: api/Users/getvisiblecontacts
        [Authorize]
        [HttpGet("getvisiblecontacts")]
        public async Task<List<Data.User>> GetVisibleContacts(int startIndex, int count) =>
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
        public async Task<int> GetContactsCount() => await _context.Users.CountAsync();

        // GET: api/Users/TwitterSignIn
        [HttpGet("TwitterSignIn")]
        public async Task TwitterSignIn(bool isPersistent) =>  await HttpContext
            .ChallengeAsync(TwitterDefaults.AuthenticationScheme, GetAuthenticationProperties(isPersistent));

        // GET: api/Users/FacebookSignIn
        [HttpGet("FacebookSignIn")]
        public async Task FacebookSignIn(bool isPersistent) => await HttpContext
            .ChallengeAsync(FacebookDefaults.AuthenticationScheme, GetAuthenticationProperties(isPersistent));
        // GET: api/Users/GoogleSignIn
        [HttpGet("GoogleSignIn")]
        public async Task GoogleSignIn(bool isPersistent) => await HttpContext
            .ChallengeAsync(GoogleDefaults.AuthenticationScheme, GetAuthenticationProperties(isPersistent));

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

        public AuthenticationProperties GetAuthenticationProperties(bool isPersistent = false)
        {
            return new AuthenticationProperties()
            {
                IsPersistent = isPersistent,
                //ExpiresUtc = DateTime.UtcNow.AddMinutes(10),
                RedirectUri = "/profile"
            };
        }

        [HttpGet("notauthorized")]
        public IActionResult NotAuthorized() => Unauthorized();

        //Migrating to JWT Authorization...
        private string GenerateJwtToken(Data.User user)
        {
            //getting the secret key
            string secret = _configuration["JWTSettings:Secret"];
            var key = Encoding.ASCII.GetBytes(secret);

            //create claims
            var claimEmail = new Claim(ClaimTypes.Email, user.EmailAddress);
            var claimNameIdentifier = new Claim(ClaimTypes.NameIdentifier, user.Id.ToString());

            //create claimsIdentity
            var claimsIdentity = new ClaimsIdentity(new[] { claimEmail, claimNameIdentifier }, "serverAuth");

            // generate token that is valid for 7 days
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            //creating a token handler
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            //returning the token back
            return tokenHandler.WriteToken(token);
        }

        [HttpPost("authenticatejwt")]
        public async Task<ActionResult<AuthenticationResponse>> AuthenticateJWT(AuthenticationRequest authenticationRequest)
        {
            string token = string.Empty;

            //authenticationRequest.Password = Utilities.Encrypt(authenticationRequest.Password);
            Data.User loggedInUser = await _context.Users
                        .Where(u => u.EmailAddress == authenticationRequest.EmailAddress && u.Password == authenticationRequest.Password)
                        .FirstOrDefaultAsync();

            if (loggedInUser is not null)
            {
                token = GenerateJwtToken(loggedInUser);
            }
            return await Task.FromResult(new AuthenticationResponse() { Token = token });
        }
        
        [HttpPost("getuserbyjwt")]
        public async Task<ActionResult<Data.User>> GetUserByJWT([FromBody] string jwtToken)
        {
            try
            {
                //getting the secret key
                string secret = _configuration["JWTSettings:Secret"];
                var key = Encoding.ASCII.GetBytes(secret);

                //preparing the validation parameters
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;

                //validating the token
                var principle = tokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out securityToken);
                var jwtSecurityToken = (JwtSecurityToken)securityToken;

                if (jwtSecurityToken != null && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    //returning the user if found
                    var userId = principle.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    return await _context.Users.Where(u => u.Id == Convert.ToInt64(userId)).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                //logging the error and returning null
                Console.WriteLine("Exception : " + ex.Message);
                return null;
            }
            //returning null if token is not validated
            return null;
        }
    }
}
