using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
    public class Grade : DomainObject
    {
       
        public int Number { get; set; }
        //Navigation Properties
        public ICollection<Classroom> Classrooms { get; set; }
        public ICollection<Subject> Subjects { get; set; }

       
    }

}
