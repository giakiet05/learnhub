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
        public string SubjectId { get; set; }
        public string SemesterId { get; set; }
        public string ClassroomId { get; set; }
        [ForeignKey("AcademicYear")]
        public string YearId { get; set; }
        public DateTime ExamDate { get; set; }
        public string ExamType { get; set; }
        public string? ExamRoom { get; set; }
        //Navigation Properties
        public Subject Subject { get; set; }
        public Semester Semester { get; set; }
        public Classroom Classroom { get; set; }
        public AcademicYear AcademicYear { get; set; }
    }
}
