using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
    public class StudentPlacement
    {
        [ForeignKey("AcademicYear")]
        public Guid ClassroomId { get; set; }
        public Guid StudentId { get; set; }

        //Navigation Properties
        public Classroom Classroom { get; set; }
        public Student Student { get; set; }
      
    }
}
