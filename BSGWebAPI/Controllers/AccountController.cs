using BSGWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BSGWebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtSettings jwtSettings;
        public AccountController(JwtSettings jwtSettings)
        {
            this.jwtSettings = jwtSettings;
        }
        private IEnumerable<Users> logins = new List<Users>() {
            new Users() {
                Id = Guid.NewGuid(),
                EmailId = "admin@gmail.com",
                UserName = "Admin",
                Password = "Admin",
            },
            new Users() {
                Id = Guid.NewGuid(),
                EmailId = "user@gmail.com",
                UserName = "User1",
                Password = "User1",
            }
        };
        [HttpPost]
        public IActionResult GetToken(UserLogins userLogins)
        {
            try
            {
                var UserTokens = new UserTokens();
                var Token = new Tokens();
                var ValidUser = logins.Any(x => x.UserName.Equals(userLogins.UserName, StringComparison.OrdinalIgnoreCase));
                if (!ValidUser)
                {
                    return BadRequest("Invalid UserName");                    
                }
                var ValidPassword = logins.Any(x => x.Password.Equals(userLogins.Password, StringComparison.OrdinalIgnoreCase));
                if (ValidPassword)
                {
                    var user = logins.FirstOrDefault(x => x.UserName.Equals(userLogins.UserName, StringComparison.OrdinalIgnoreCase));
                    UserTokens = JwtHelpers.JwtHelpers.GenTokenkey(new UserTokens()
                    {
                        EmailId = user.EmailId,
                        GuidId = Guid.NewGuid(),
                        UserName = user.UserName,
                        Id = user.Id,
                    }, jwtSettings);

                    Token = new Tokens()
                    {
                        Token = UserTokens.Token,
                        UserName = UserTokens.UserName
                    };
                }
                else
                {
                    return BadRequest("Invalid Password");
                }
                return Ok(Token);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Get List of UserAccounts
        /// </summary>
        /// <returns>List Of UserAccounts</returns>
        [HttpGet]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetList()
        {
            return Ok(logins);
        }
    }
}
