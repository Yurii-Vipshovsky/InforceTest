using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShorderLink.Data;
using ShorderLink.Models;

namespace ShorderLink.Controllers
{
    public class UsersController : Controller
    {
        private readonly ShorderLinkContext _context;
        private readonly IConfiguration _configuration;
        private const int TOKEN_LIFE_TIME = 15;//in minutes

        public UsersController(ShorderLinkContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private ClaimsIdentity GetIdentity(string email, string password)
        {
            User user = _context.User.FirstOrDefault(x => x.Email == email && x.Password == password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("userId", user.Id.ToString()),
                    new Claim("userRole", user.Role)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token");
                return claimsIdentity;
            }
            return null;
        }

        public class LoginUser
        {
            public string email { get; set; }
            public string password { get; set; }
        }

        /// <summary>
        /// User login using Email and Password
        /// Creates JWT Token for 15 minutes with user info
        /// </summary>
        /// <param name="loginUser">Email and Password</param>
        /// <returns>200 with user token</returns>
        /// <returns>400 if Invalid username or password.</returns>
        [HttpPost]
        // Post: Users/Login
        public async Task<IActionResult> Login([FromForm] LoginUser loginUser)
        {
            var identity = GetIdentity(loginUser.email, loginUser.password);
            if (identity == null)
            {
                return BadRequest("Invalid username or password.");
            }
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: _configuration["JwtSetting:Issuer"],
                    audience: _configuration["JwtSetting:Audience"],
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(TOKEN_LIFE_TIME)),
                    signingCredentials: new SigningCredentials(
                                        new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtSetting:Key"])),
                                        SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
            };
            return Ok(response);
        }

        public class RegisterUser
        {
            public string name { get; set; }
            public string email { get; set; }
            public string password { get; set; }
        }

        /// <summary>
        /// Register new user in "user" Role
        /// Create user with admin Role impossible only change in data base
        /// </summary>
        /// <param name="getUser">User name, Email, Password</param>
        /// <returns>200 if user registered</returns>
        /// <returns>400 if user with same email exists</returns>
        // POST: Users/Register
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterUser getUser)
        {
            User user = new User() { Email = getUser.email, Password = getUser.password, Name = getUser.name, Role = "user" };
            if ((_context.User?.Any(e => e.Email == getUser.email)).GetValueOrDefault())
            {
                return BadRequest("User with this email is already registered");
            }
            user.Id = Guid.NewGuid();
            _context.Add(user);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
