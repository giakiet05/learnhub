using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
    public class AcademicYear
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }

        //Navigation Properties
        public ICollection<YearResult> YearResults { get; set; }
        public ICollection<StudentPlacement> StudentPlacement { get; set; }
        public ICollection<TeachingAssignment> TeachingAssignments { get; set; }
        public ICollection<ExamSchedule> ExamSchedules { get; set; }
        public ICollection<SubjectResult> SubjectResults { get; set; }
    }
}
