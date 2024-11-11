using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
    public class YearResult
    {
        [ForeignKey("AcademicYear")]
        public string YearId { get; set; }
        public string StudentId { get; set; }

        public double? YearAvgScore { get; set; }
        public double? FirstSemAvgScore { get; set; }
        public double? SecondSemAvgScore { get; set; }
        public string? Result { get; set; } // Kết quả = hạnh kiểm + học lực
        public string? Conduct { get; set; } //Hạnh kiểm
        public string? AcademicPerformance { get; set; } //Học lực
        //Navigation Properties
        public AcademicYear AcademicYear { get; set; }
        public Student Student { get; set; }
    }

}
