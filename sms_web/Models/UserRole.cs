using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace sms_web.Models
{   [Table("UserRole")]
    public class UserRole
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
    }
}
