using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
    public class AcademicYear : DomainObject
    {
      
        public string Name { get; set; }

        //Navigation Properties
        public ICollection<YearResult> YearResults { get; set; }
        public ICollection<Classroom> Classrooms { get; set; }
        public ICollection<SubjectResult> SubjectResults { get; set; }
    }
}
