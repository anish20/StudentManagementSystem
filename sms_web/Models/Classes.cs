using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace sms_web.Models
{
    [Table("Classes")]
    public class Classes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Added_On { get; set; }
        public DateTime Updated_On { get; set; }
        public string Added_By { get; set; }
        public string Updated_By { get; set; }
        public int? Status { get; set; }

    }
}

