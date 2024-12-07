using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
    public class ExamSchedule : IAdminId
    {
      
        public Guid SubjectId { get; set; }
     
        public Guid ClassroomId { get; set; }

        public string Semester { get; set; }

        public DateTime? ExamDate { get; set; }
        public string? ExamType { get; set; }
        public string? ExamRoom { get; set; }
        public Guid? AdminId { get; set; }
        public Admin Admin { get; set; }
        //Navigation Properties
        public Subject Subject { get; set; }
      
        public Classroom Classroom { get; set; }
    
      
    }
}
