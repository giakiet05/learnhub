using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
    public class StudentPlacement
    {
        public string ClassroomId { get; set; }
        public string StudentUsername { get; set; }

        [ForeignKey("AcademicYear")]
        public string YearId { get; set; }
        //Navigation Properties
        public Classroom Classroom { get; set; }
        public Student Student { get; set; }
        public AcademicYear AcademicYear { get; set; }
    }
}
