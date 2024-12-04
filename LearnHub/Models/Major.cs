using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
  public  class Major : DomainObject
    {
        public string Name { get; set; }
        public string? UserId { get; set; }
        public User User { get; set; }
        //Navigation props
        public ICollection<Subject> Subjects {  get; set; }
        public ICollection<Teacher> Teachers { get; set; }
    }
}
