using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace sms_web.Models
{   [Table("Students")]
    public class Student
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Please Enter Full Name")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Please Enter Father Name")]
        public string FatherName { get; set; }
        [Required(ErrorMessage = "Please Enter Mother Name")]
        public string MotherName { get; set; }
        [Required(ErrorMessage = "Please select Date of Birth")]
        public string DOB { get; set; }
        
        public int AdmitionClass { get; set; }
        [Required(ErrorMessage = "Please select gender")]
        public string Gender { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public string StudentId { get; set; }
        public string ProfileImage { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        public DateTime Admited_On { get; set; }
        public string Admited_By { get; set; }
        public int Status { get; set; }
       

    }
}
