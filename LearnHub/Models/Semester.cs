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
        public Guid Id { get; set; }
        public string Name { get; set; }
        //Navigation Properties
        //public ICollection<ExamSchedule> ExamSchedules { get; set; }
        //public ICollection<SubjectResult> SubjectResults { get; set; }
    }
}
