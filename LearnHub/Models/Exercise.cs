using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
    public class Exercise: DomainObject
    {
      

        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? TeacherId { get; set; }
        public string? SubjectId { get; set; }
        public string? ClassroomId { get; set; }
        public DateTime? PublishTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        //Navigation Properties
        public Teacher Teacher { get; set; }
        public Subject Subject { get; set; }
        public Classroom Classroom { get; set; }

    }

}
