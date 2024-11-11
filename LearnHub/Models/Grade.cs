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
       
        public string? Name { get; set; }
        //Navigation Properties
        public ICollection<Classroom> Classrooms { get; set; }
    }

}
