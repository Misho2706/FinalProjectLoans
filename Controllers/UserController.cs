using FinalProjectLoans.Helpers;
using FinalProjectLoans.Model.Domain;
using FinalProjectLoans.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinalProjectLoans.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private User currentUser = new User();
        private readonly IUserService _userService;
        private readonly AppSettings _appSettings;

        public UserController(IUserService userService, IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _appSettings = appSettings.Value;

        }
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(string username, string password)
        {
            var user = _userService.LogIn(username, password);
            if (user == null)
                return BadRequest(new { message = "Incorect UserName or Password" });

            var tokenString = GenerateToken(user);
            currentUser = user;
            
            return Ok(
                new
                {
                    Id = user.Id,
                    FirsName = user.FirstName,
                    LastName = user.LastName,
                    Token = tokenString
                });

        }
        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AppSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.FirstName.ToString())
                    }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult RegisterUser(User newUser)
        {
            var user = _userService.RegisterUser(newUser);
            if (user == null)
            {
                
                return BadRequest(new { message = "username already exists" });
            }
            return Ok(user);
        }
        [HttpPost("newloan")]
        public IActionResult CreateLoan(Loan newLoan)
        {
            var loan = _userService.CreateLoan(currentUser, newLoan);
            if(loan == null) { return BadRequest("you can't request new loan"); }
            return Ok(loan);
        }
        [HttpPut("editloan")]
        public IActionResult EditLoan(Loan editedLoan)
        {
            var loan = _userService.EditLoan(currentUser,editedLoan);
            if (loan == null) return BadRequest("you can't edit this loan");
            return Ok(loan);
        }

        [HttpDelete("deleteloan")]
        public IActionResult DeleteLoan(Loan loan)
        {
            var _loan = _userService.DeleteLoan(currentUser, loan);
            if (_loan == null) return BadRequest("you can't delete this loan");
            return Ok(loan);
        }

        [HttpGet("checkloans")]
        public IActionResult CheckLoans()
        {
            return Ok(_userService.GetAllLoans(currentUser));
        }

    }
}
