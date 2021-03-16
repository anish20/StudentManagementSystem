using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sms_web.Models;
using sms_web.MyDbContext;

namespace sms_web.Controllers
{
    public class StudentController : Controller
    {
        public readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostenv;
        public StudentController(ApplicationDbContext context, IWebHostEnvironment hostenv)
        {
            _context = context;
            _hostenv = hostenv;

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
                    List<Student> list = await _context.Students.ToListAsync();
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

        public async Task<IActionResult> NewStudent(Student student=null)
        {
            var tokens = HttpContext.Session.GetString("JWtTokens");
            var role = HttpContext.Session.GetString("UserRole");
            string TitleHeader = "Add New Student";
            string buttonText = "Save";
            string buttonBgColor = "btn-primary";
            string disabled = "";
            if (HttpContext.Session != null && tokens != null && role == "Admin")
            {
                if (student.Id != 0)
                {
                     TitleHeader = "Update Student";
                    buttonText = "Update";
                    buttonBgColor = "btn-warning";
                    disabled = "Disabled";

                    var findStudent = await _context.Students.Where(a => a.Id == student.Id).FirstOrDefaultAsync();
                    student.FullName = findStudent.FullName;
                    student.FatherName = findStudent.FatherName;
                    student.MotherName = findStudent.MotherName;
                    student.DOB = findStudent.DOB;
                    student.Gender = findStudent.Gender;
                    student.AdmitionClass = findStudent.AdmitionClass;
                    student.Address = findStudent.Address;
                    student.ContactNo = findStudent.ContactNo;
                    student.ProfileImage = findStudent.ProfileImage;
                    
                }
                ViewBag.SetTitleHeader = TitleHeader;
                ViewBag.SetButtonText = buttonText;
                ViewBag.SetButtonBgColor = buttonBgColor;
                ViewBag.SetDisabled= disabled;
               
               

                return View(student);
            }
            else
            {
                return RedirectToAction("LoginAuthentication", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StudentSaveAndUpdate(Student student)
        {
            var tokens = HttpContext.Session.GetString("JWtTokens");
            var role = HttpContext.Session.GetString("UserRole");
            int result = 0;
            try
            {
                if (HttpContext.Session != null && tokens != null && role == "Admin")
                {
                    if (ModelState.IsValid && student.Id == 0)
                    {
                        string uniqueFileName = UploadedFile(student);
                        string StdentId = "S" + StudentIdGenerate(9);
                        student.ProfileImage = uniqueFileName;
                        student.Status = 1;
                        student.Admited_On = DateTime.Now;
                        student.StudentId = StdentId;
                        student.Admited_By = role;
                        _context.Students.Add(student);
                       result= await _context.SaveChangesAsync();
                        if (result > 0)
                        {
                            ViewBag.SuccessMessage = "Student Registration Successfully";
                            ModelState.Clear();
                        }
                        
                    }
                    else
                    {
                        var findStudent = _context.Students.Where(a => a.Id == student.Id).FirstOrDefault();

                        if (student.Image != null)
                        {
                            
                            string filePath = Path.Combine(_hostenv.WebRootPath, "admin/StudentImage", findStudent.ProfileImage);
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);

                            }

                            string uniqueFileName = UploadedFile(student);
                            findStudent.ProfileImage = uniqueFileName;
                        }
                        findStudent.FullName = student.FullName;
                        findStudent.FatherName = student.FatherName;
                        findStudent.MotherName = student.FullName;
                        findStudent.DOB = student.DOB;
                        findStudent.Address = student.Address;
                        findStudent.ContactNo = student.ContactNo;
                        _context.Students.Update(findStudent);
                        result = _context.SaveChanges();
                        if (result > 0)
                        {
                            ViewBag.SuccessMessage = "Student Record Updated Successfully";
                            return RedirectToAction("Index", "Student");

                        }

                    }
                }
                else
                {
                    return RedirectToAction("LoginAuthentication", "Account");
                }
            }
            catch (Exception exp)
            {

                throw exp;
            }

            return View("NewStudent");

        }

        private string UploadedFile(Student model)
        {
            string uniqueFileName = null;

            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(_hostenv.WebRootPath, "admin/StudentImage");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        private static readonly Random _rdm = new Random();
        private static string StudentIdGenerate(int digits)
        {
            if (digits <= 1) return "";

            var _min = (int)Math.Pow(10, digits - 1);
            var _max = (int)Math.Pow(10, digits) - 1;
            return _rdm.Next(_min, _max).ToString();
        }
    }
}