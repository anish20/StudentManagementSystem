using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sms_web.Models;
using sms_web.MyDbContext;

namespace sms_web.Controllers
{
    public class ClassMasterController : Controller
    {
        public readonly ApplicationDbContext _context;
        public ClassMasterController(ApplicationDbContext context)
        {
            _context = context;

        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var tokens = HttpContext.Session.GetString("JWtTokens");
            var role = HttpContext.Session.GetString("UserRole");
            if (HttpContext.Session != null && tokens != null && role == "Admin")
            {

                try
                {
                    List<Classes> list = await _context.Classes.ToListAsync();
                    if (list.Count != 0)
                    {
                        return View(list);
                    }
                    else
                    {
                        ViewBag.Data = "Sorry Data Not found";
                        return View(list);
                    }
                }
                catch (Exception exp)
                {

                    throw exp;
                }

            }
            else
            {
                return RedirectToAction("LoginAuthentication", "Account");
            }


        }

        public IActionResult AddNewClass(Classes classes = null)
        {
            string titleHeader = "Add New Class";
            string buttonText = "Submit";
            string buttonBgColor = "btn btn-primary";
            string textIcon = "fa fa-save";
            var tokens = HttpContext.Session.GetString("JWtTokens");
            var role = HttpContext.Session.GetString("UserRole");
            if (HttpContext.Session != null && tokens != null && role == "Admin")
            {
                if (classes.Id != 0)
                {
                    titleHeader = "Update Class";
                    buttonText = "Update";
                    buttonBgColor = "btn btn-warning";
                    textIcon = "fa fa-edit";
                    var getClass = _context.Classes.Where(a => a.Id == classes.Id).FirstOrDefault();
                    classes.Name = getClass.Name;
                    classes.Description = getClass.Description;
                }
                ViewBag.SetHeader = titleHeader;
                ViewBag.SetButtonText = buttonText;
                ViewBag.SetButtonBgColor = buttonBgColor;
                ViewBag.SetTextIcon = textIcon;
                return View(classes);
            }
            else
            {
                return RedirectToAction("LoginAuthentication", "Account");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveAndUpdateClass(Classes classes)
        {
            var tokens = HttpContext.Session.GetString("JWtTokens");
            var role = HttpContext.Session.GetString("UserRole");
            int result = 0;
            if (HttpContext.Session != null && tokens != null && role == "Admin")
            {
                if (classes.Id == 0)
                {
                    var findData = _context.Classes.Where(a => a.Name == classes.Name).FirstOrDefault();
                    if (findData != null)
                    {
                        ViewBag.Error = "Sorry this class Already Exist";
                        return View("AddNewClass");
                    }
                    else
                    {
                        classes.Added_By = role;
                        classes.Added_On = DateTime.Now;
                        classes.Status = 1;
                        _context.Classes.Add(classes);
                        result = _context.SaveChanges();
                        return RedirectToAction("Index", "ClassMaster");
                    }
                }
                else
                {
                    var getData = _context.Classes.Where(a => a.Id == classes.Id).FirstOrDefault();
                    getData.Name = classes.Name;
                    getData.Description = classes.Description;
                    getData.Updated_On = DateTime.Now;
                    getData.Updated_By = role;
                    _context.Classes.Update(getData);
                    result = _context.SaveChanges();
                    return RedirectToAction("Index", "ClassMaster");

                }
            }
            else
            {
                return RedirectToAction("LoginAuthentication", "Account");
            }

        }

        [HttpGet]
        public IActionResult ClassActiveAndDeactive(Classes classes)
        {
            var tokens = HttpContext.Session.GetString("JWtTokens");
            var role = HttpContext.Session.GetString("UserRole");
            int result = 0;
            if (HttpContext.Session != null && tokens != null && role == "Admin")
            {
                if (classes.Id != 0)
                {
                    var findData = _context.Classes.Where(a => a.Id == classes.Id).FirstOrDefault();
                    if (findData.Status == 0)
                    {
                        //Neeed to Active
                        findData.Status = 1;
                        _context.Classes.Update(findData);
                        result = _context.SaveChanges();
                        return RedirectToAction("Index", "ClassMaster");

                    }
                    else
                    {
                        //Neeed to DeActive
                        findData.Status = 0;
                        _context.Classes.Update(findData);
                        result = _context.SaveChanges();
                        return RedirectToAction("Index", "ClassMaster");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "ClassMaster");
                }
            }
            else
            {
                return RedirectToAction("LoginAuthentication", "Account");
            }
        }

        //Delete Class
        [HttpGet]
        public async Task<int> DeleteClass(int id)
        {

            int result = 0;
            var tokens = HttpContext.Session.GetString("JWtTokens");
            var role = HttpContext.Session.GetString("UserRole");
           
            if (HttpContext.Session != null && tokens != null && role == "Admin")
            {
                try
                {
                    _context.Classes.Remove(new Classes { Id = id });
                    result = _context.SaveChanges();
                }
                catch (Exception exp)
                {

                    throw exp;
                }
                
            }
            else
            {
                return 404;
            }
            return result;
        }

    }
}