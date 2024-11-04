using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
    public class ExamSchedule
    {
        [ForeignKey("AcademicYear")]
        public Guid YearId { get; set; }
        public Guid SubjectId { get; set; }
        //public Guid SemesterId { get; set; }
        public Guid ClassroomId { get; set; }

        public string Semester { get; set; }

        public DateTime? ExamDate { get; set; }
        public string ExamType { get; set; }
        public string? ExamRoom { get; set; }
        //Navigation Properties
        public Subject Subject { get; set; }
        //public Semester Semester { get; set; }
        public Classroom Classroom { get; set; }
        public AcademicYear AcademicYear { get; set; }
      
    }
}
