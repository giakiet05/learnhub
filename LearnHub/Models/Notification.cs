using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
    public class Notification
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey("User")]
        public string Creator { get; set; }
        public string Title { get; set; }

        public string ClassroomId { get; set; }

        public string? Content { get; set; }
        public DateTime? PublishDate { get; set; }

        //Navigation Properties
        public Classroom Classroom { get; set; }
        public User User { get; set; }
    }
}
