using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
    public class Semester
    {
        [Key]
        public string SemesterId { get; set; }
        public string SemesterName { get; set; }
        //Navigation Properties
        public ICollection<ExamSchedule> ExamSchedules { get; set; }
        public ICollection<YearResult> YearResults { get; set; }
    }
}
