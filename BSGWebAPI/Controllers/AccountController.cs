using BSGWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using BS = BCrypt.Net.BCrypt;

namespace BSGWebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IConfiguration Configuration;
        private readonly JwtSettings jwtSettings;
        public AccountController(JwtSettings jwtSettings, IConfiguration _configuration)
        {
            this.jwtSettings = jwtSettings;
            Configuration = _configuration;
        }
        
        [HttpPost]
        public IActionResult GetToken(UserLogins userLogins)
        {
            try
            {
                var UserTokens = new UserTokens();
                var Token = new Tokens();
                bool ValidUser = ValidateUser(userLogins.UserName);
                if (!ValidUser)
                {
                    return BadRequest("Invalid UserName");
                }
                bool ValidPassword = ValidatePassword(userLogins);
                if (ValidPassword)
                {
                    var user = logins(userLogins);
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

        #region Functions
        private bool ValidateUser(string UserName)
        {
            bool isValidUser = false;
            string strConn = this.Configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection openCon = new SqlConnection(strConn))
            {
                string selectInquiry = "SELECT UserName FROM USERS WHERE UserName = @UserName";
                using (SqlCommand querySelectInquiry = new SqlCommand(selectInquiry))
                {
                    querySelectInquiry.Connection = openCon;
                    querySelectInquiry.Parameters.Add("@UserName", SqlDbType.VarChar, 50).Value = UserName;
                    openCon.Open();

                    using (SqlDataReader reader = querySelectInquiry.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            isValidUser = true;
                        }
                    }
                }
            }
            return isValidUser;
        }
        private bool ValidatePassword(UserLogins userLogins)
        {
            bool isPasswordValid = false;
            string strConn = this.Configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection openCon = new SqlConnection(strConn))
            {
                string selectInquiry = "SELECT Password FROM USERS WHERE UserName = @UserName";
                using (SqlCommand querySelectInquiry = new SqlCommand(selectInquiry))
                {
                    querySelectInquiry.Connection = openCon;
                    querySelectInquiry.Parameters.Add("@UserName", SqlDbType.VarChar, 50).Value = userLogins.UserName;
                    openCon.Open();

                    using (SqlDataReader reader = querySelectInquiry.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {
                                string fromDBHashedPassword = reader["Password"].ToString();
                                isPasswordValid = BS.Verify(userLogins.Password, fromDBHashedPassword);
                            }
                        }
                    }
                }
            }
            return isPasswordValid;
        }
        private Users logins(UserLogins userLogins)
        {
            Users users = new Users();
            string strConn = this.Configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection openCon = new SqlConnection(strConn))
            {
                string selectInquiry = "SELECT * FROM USERS WHERE UserName = @UserName";
                using (SqlCommand querySelectInquiry = new SqlCommand(selectInquiry))
                {
                    querySelectInquiry.Connection = openCon;
                    querySelectInquiry.Parameters.Add("@UserName", SqlDbType.VarChar, 50).Value = userLogins.UserName;
                    openCon.Open();

                    using (SqlDataReader reader = querySelectInquiry.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {
                                users.Id = new Guid(reader["Id"].ToString());
                                users.UserName = reader["UserName"].ToString();
                                users.EmailId = reader["EmailId"].ToString();
                            }
                        }
                    }
                }
            }
            return users;
        }
        #endregion Functions
    }
}
