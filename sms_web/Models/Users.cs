using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace sms_web.Models
{   [Table("Users")]
    public class Users
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
        public int Status { get; set; }

    }
}
