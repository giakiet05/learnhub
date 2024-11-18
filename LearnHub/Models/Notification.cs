using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
    public class Notification : DomainObject
    {
       
        public string? Title { get; set; }
        [ForeignKey("User")]
        public string? CreatorId { get; set; }

        public string? ClassroomId { get; set; }

        public string? Content { get; set; }
        public DateTime? PublishDate { get; set; } = DateTime.Now;

        //Navigation Properties
        public Classroom Classroom { get; set; }
        public User User { get; set; }
    }
}
