using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
    public class StudentPlacement : IAdminId
    {
        public string ClassroomId { get; set; }
        public string StudentId { get; set; }
        public string? AdminId { get; set; }
        public Admin Admin { get; set; }

        //Navigation Properties
        public Classroom Classroom { get; set; }
        public Student Student { get; set; }
      
    }
}
