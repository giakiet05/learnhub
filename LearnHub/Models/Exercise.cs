using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
    public class Exercise
    {
        [Key]
        public string ExerciseId { get; set; }

        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string TeacherUsername { get; set; }
        public string SubjectId { get; set; }
        public string ClassroomId { get; set; }
        //Navigation Properties
        public Teacher Teacher { get; set; }
        public Subject Subject { get; set; }
        public Classroom Classroom { get; set; }
    }

}
