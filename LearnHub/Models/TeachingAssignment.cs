using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
    public class TeachingAssignment
    {
        public string ClassroomId { get; set; }
        public string SubjectId { get; set; }
        public string TeacherUsername { get; set; }
        [ForeignKey("AcademicYear")]
        public string YearId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //Navigation Properties
        public Classroom Classroom { get; set; }
        public Subject Subject { get; set; }
        public Teacher Teacher { get; set; }
        public AcademicYear AcademicYear { get; set; }
    }
}
