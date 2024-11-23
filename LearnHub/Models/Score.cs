using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
    public class Score
    {
        //-----Composite Key------
        [ForeignKey("AcademicYear")]
        public string YearId { get; set; }
        public string SubjectId { get; set; }
        public string StudentId { get; set; }
        public string Semester { get; set; }
   //     public string Type { get; set; } // loại, TX, GK, CK,...
        //-----Composite Key------

        public double? GKScore { get; set; } //điểm GK
        public double? CKScore { get; set; } // điểm CK
        public string? TXScore { get; set; } // điểm TX
       

        //Navigation Properties
        public Subject Subject { get; set; }
        public Student Student { get; set; }
        public AcademicYear AcademicYear { get; set; }
    }
}
