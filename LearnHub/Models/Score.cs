using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
    public class Score : IAdminId
    {
        //-----Composite Key------
        [ForeignKey("AcademicYear")]
        public string YearId { get; set; }
        public string SubjectId { get; set; }
        public string StudentId { get; set; }
        public string Semester { get; set; }
   //     public string Type { get; set; } // loại, TX, GK, CK,...
        //-----Composite Key------

        public double? MidTermScore { get; set; } //điểm GK
        public double? FinalTermScore { get; set; } // điểm CK
        public string? RegularScores { get; set; } // điểm TX      
        public double? AvgScore { get; set; } // điểm tb
        public string? AdminId { get; set; }
        public Admin Admin { get; set; }
        //Navigation Properties
        public Subject Subject { get; set; }
        public Student Student { get; set; }
        public AcademicYear AcademicYear { get; set; }
    }
}
