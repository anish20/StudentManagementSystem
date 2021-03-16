using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sms_web.Models;
using sms_web.MyDbContext;

namespace sms_web.Controllers
{
    //[Authorize]
    public class RoleController : Controller
    {
        public readonly ApplicationDbContext _context;
        public RoleController(ApplicationDbContext context)
        {
            _context = context;
            
        }
        
        [HttpGet]

        public async Task<List<UserRole>> GetRole()
        {
            var tokens = HttpContext.Session.GetString("JWtTokens");
            List<UserRole> list = null;

            if (tokens != null)
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens);
                list = _context.UserRoles.ToList();
                return list;
            }
            else
            {
                return list;
            }
           
        }
    }
}