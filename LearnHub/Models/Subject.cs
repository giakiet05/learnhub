using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
    public class Subject
    {
        [Key]
        public string SubjectId { get; set; }

        public string SubjectName { get; set; }

        public int? LessonNumber { get; set; }

        //Navigation Properties
        public ICollection<TeachingAssignment> TeachingAssignments { get; set; }
        public ICollection<Document> Documents { get; set; }
        public ICollection<Exercise> Exercises { get; set; }
        public ICollection<ExamSchedule> ExamSchedules { get; set; }
        public ICollection<SubjectResult> SubjectResults { get; set; }
    }
}
