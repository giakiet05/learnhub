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
        
        [ForeignKey("AcademicYear")]
        public Guid YearId { get; set; }
     
        public Guid SubjectId { get; set; }
        public Guid StudentId { get; set; }

        public string Semester { get; set; }

        public double? OralScore { get; set; }
        public double? FifteenMinScore { get; set; }
        public double? MidTermScore { get; set; }
        public double? FinalTermScore { get; set; }
        public double? AvgScore { get; set; }
        //Navigation Properties
      
        public Subject Subject { get; set; }
        public Student Student { get; set; }
        public AcademicYear AcademicYear { get; set; }
    }
}
