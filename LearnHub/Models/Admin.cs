using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LearnHub.Models
{
    [Table("Admin")]
    public class Admin : User
    {
       
        public string SchoolName { get; set; }
        public string Email { get; set; }
    }
}
