using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using sms_web.Models;
using sms_web.MyDbContext;

namespace sms_web.Controllers
{
    public class AccountController : Controller
    {
        public readonly ApplicationDbContext _context;
        public readonly IConfiguration _configuration;
        public AccountController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult LoginAuthentication()
        {
            if (HttpContext.Session != null)
            {
                HttpContext.Session.Clear();
            }
            return View();

        }

        [HttpPost]
        //[Route("LoginAuthentication")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> LoginAuthentication(Users users)
        {
            try
            {
                if (users != null)
                {
                    var findUser = await _context.Users.Where(a => a.UserName == users.UserName && a.Password == users.Password).FirstOrDefaultAsync();
                    if (findUser != null)
                    {
                        var getRole = await _context.UserRoles.Where(b => b.Id == findUser.Role).FirstOrDefaultAsync();
                        //GetToken
                        string MyToken= generateToken(findUser.UserName,_configuration);
                        HttpContext.Session.SetString("USER_LOGIN", "YES");
                        HttpContext.Session.SetString("USER_NAME", findUser.UserName);
                        HttpContext.Session.SetString("JWtTokens", MyToken);
                        HttpContext.Session.SetString("UserRole", getRole.RoleName);

                        if (findUser.Role == 1)
                        {
                            return RedirectToAction("AdminDashboard","Dashboard");
                        }
                        if (findUser.Role == 2)
                        {
                            return RedirectToAction("User", "Dashboard");
                        }
                        if (findUser.Role == 3)
                        {
                            return RedirectToAction("ManagerDashboard", "Dashboard");
                        }
                    }
                    else
                    {
                        ViewBag.Error = "Sorry Username and Password Incorrect";
                       
                    }
                }
                return View();
            }
            catch (Exception exp)
            {

                throw exp;
            }
        }
    
        //Generate token
        public static string generateToken(string username, IConfiguration _configuration)
        {
            string tokens = "";
            try
            {
                var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"]),
                            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                            new Claim("Username",username)
                        };
                //Key generation
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: signIn
                    );
                var tok = new JwtSecurityTokenHandler().WriteToken(token);
                tokens = tok;

            }
            catch (Exception exp)
            {

                throw exp;
            }
            return tokens;
        }
        
        //Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("USER_NAME");
            HttpContext.Session.Remove("USER_LOGIN");
            HttpContext.Session.Remove("JwtTokens");
            HttpContext.Session.Clear();

            return RedirectToAction("LoginAuthentication","Account");
        }
    }
}