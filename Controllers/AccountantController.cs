using FinalProjectLoans.Helpers;
using FinalProjectLoans.Model.Domain;
using FinalProjectLoans.Service;
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
    public class AccountantController : Controller
    {
        private Accountant currentUser = new Accountant();
        private readonly IAccountantService _accountantservice;
        private readonly AppSettings _appSettings;

        public AccountantController(IAccountantService accountantService, IOptions<AppSettings> appSettings)
        {
            _accountantservice = accountantService;
            _appSettings = appSettings.Value;

        }
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(string username, string password)
        {
            var user = _accountantservice.LogIn(username, password);
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
        private string GenerateToken(Accountant user)
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
        [HttpGet("getallloans")]
        public IActionResult GetAllLoans()
        {
            return Ok(_accountantservice.GetAllLoans());
        }

        [HttpPut("changeuserstatus")]
        public IActionResult ChangeUserStatus(int id, bool isBlocked)
        {
            var user = _accountantservice.ChangeUserStatus(id, isBlocked);
            if (user == null)
            {
                return BadRequest("user doesn't exist");
            }
            return Ok(user);
        }
        [HttpDelete("deleteuserloan")]
        public IActionResult DeleteUserLoan(int id)
        {
            var loan = _accountantservice.DeleteUserLoan(id);
            if (loan == null) return BadRequest("loan doesn't exist");
            return Ok(loan);
        }
        [HttpPut("edituserloan")]
        public IActionResult EditUserLoan(Loan loanToEdit)
        {
            var loan = _accountantservice.EditUserLoan(loanToEdit);
            if (loan == null)
            {
                return BadRequest("loan doesn't exist");
            }
            return Ok(loan);
        }
    }
}
