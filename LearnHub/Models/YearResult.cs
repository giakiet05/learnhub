using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
    public class YearResult
    {
        public string YearId { get; set; }
        public string StudentUsername { get; set; }

        public double YearAverageScore { get; set; }
        public double Semester1AverageScore { get; set; }
        public double Semester2AverageScore { get; set; }
        public string Result { get; set; } // Kết quả
        public string Conduct { get; set; } //Hạnh kiểm
        public string AcademicPerformance { get; set; } //Học lực
        //Navigation Properties
        public AcademicYear AcademicYear { get; set; }
        public Student Student { get; set; }
    }

}
