﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
    public class SemesterResult
    {

        //----Composite Key------
        [ForeignKey("AcademicYear")]
        public string YearId { get; set; }
        public string StudentId { get; set; }
        public string Semester {  get; set; }

        //public double? YearAvgScore { get; set; }
        public string? Conduct { get; set; } //Hạnh kiểm
        public string? AcademicPerformance { get; set; } //Học lực
        public double? AvgScore { get; set; } // điểm trung bình
        public int? AuthorizedLeaveDays { get; set; } // nghỉ có phép
        public int? UnauthorizedLeaveDays { get; set; } // nghỉ không phép
        public string? Result {  get; set; } // danh hiệu
        public string? UserId { get; set; }
        public User User { get; set; }
        //Navigation Properties
        public AcademicYear AcademicYear { get; set; }
        public Student Student { get; set; }
    }

}
