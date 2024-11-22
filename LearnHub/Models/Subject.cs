using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
    public class Subject : DomainObject
    {
        
        public string? Name { get; set; }

        public int? LessonNumber { get; set; }

        public string? GradeId { get; set; }

        public string? MajorId { get; set; }

        //Navigation Properties
        public Grade Grade { get; set; }
        public Major Major { get; set; }
        public ICollection<TeachingAssignment> TeachingAssignments { get; set; }
        public ICollection<Document> Documents { get; set; }
       
        public ICollection<ExamSchedule> ExamSchedules { get; set; }
        public ICollection<SubjectResult> SubjectResults { get; set; }
    }
}
