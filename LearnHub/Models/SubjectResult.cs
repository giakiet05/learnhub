using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
    public class SubjectResult
    {
        //-----Composite Key------
        [ForeignKey("AcademicYear")]
        public string YearId { get; set; }
        public string SubjectId { get; set; }
        public string StudentId { get; set; }
        public string Semester { get; set; }
        //-----Composite Key------

        //public Score MidTermScore { get; set; }
        //public Score FinalTermScore { get; set; }
        //public Score AvgScore { get; set; }

        public ICollection<Score> Scores { get; set; } // Unified collection for all scores

        //Navigation Properties
        public Subject Subject { get; set; }
        public Student Student { get; set; }
        public AcademicYear AcademicYear { get; set; }
    }
}
