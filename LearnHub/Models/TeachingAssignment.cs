using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
    public class TeachingAssignment
    {
        public string ClassroomId { get; set; }
        public string SubjectId { get; set; }
        public string TeacherId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public string? Weekday { get; set; }
        public string? Period { get; set; }
        //Navigation Properties
        public Classroom Classroom { get; set; }
        public Subject Subject { get; set; }
        public Teacher Teacher { get; set; }
    }
}
