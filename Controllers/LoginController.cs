using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestAPI.Models;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public WebApiContext db;
        public LoginController (WebApiContext DB)
        {
            db = DB;
        }
        [HttpGet]
        [Route("ErrorForbidden")]
        public IActionResult ErrorForbidden()
        {
            return StatusCode(403, "You dont have permission");
        }
        [HttpGet]
        [Route("ErrorNotLogIn")]
        public IActionResult ErrorNotLogIn()
        {
            return Unauthorized();
        }


        [HttpPost]
        //public IActionResult AuthenticationAsync([FromBody] Account acc)
        public IActionResult Authentication([FromHeader] string Authorization)
        {
            string[] str = Authorization.Split(' ');
            string decodeAuthToken = Encoding.UTF8.GetString(Convert.FromBase64String(str[1]));
            string[] usernamePassArray = decodeAuthToken.Split(':');
            string username = usernamePassArray[0];
            string pass = usernamePassArray[1];
            var userDetail = db.Accounts.Where(x => x.UserName == username).FirstOrDefault();
            if (userDetail == null)
                return BadRequest();
            bool validPassword = BCrypt.Net.BCrypt.Verify(pass, userDetail.UserPass);
            if (validPassword)
            {
                var identity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Role, userDetail.UserRole),
                    new Claim(ClaimTypes.Name,userDetail.UserName)}, 
                    CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return Ok("Success Login");
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return NoContent();
        }
    }
}