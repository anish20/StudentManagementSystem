using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace sms_web.Controllers
{
    
    public class DashboardController : Controller
    {
        public IActionResult AdminDashboard()
        {
            var tokens= HttpContext.Session.GetString("JWtTokens");
            var  role = HttpContext.Session.GetString("UserRole");
            if (HttpContext.Session!=null && tokens != null  && role=="Admin")
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens);
                return View();
            }
            else
            {
                return RedirectToAction("LoginAuthentication", "Account");
                
            }

           
        }
        
        public IActionResult ManagerDashboard()
        {
            var tokens = HttpContext.Session.GetString("JWtTokens");
            var role = HttpContext.Session.GetString("UserRole");
            if (HttpContext.Session != null && tokens != null && role == "Manager")
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens);
                return View();
            }
            else
            {
                return RedirectToAction("LoginAuthentication", "Account");

            }
        }
    }
}