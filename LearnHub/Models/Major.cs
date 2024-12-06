using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
    public class Major : IAdminId
    {
        public Guid Id { get; set; }
        public string OriginalId { get; set; }
        public string Name { get; set; }
        public Guid? AdminId { get; set; }
        public Admin Admin { get; set; }
        //Navigation props
        public ICollection<Subject> Subjects { get; set; }
        public ICollection<Teacher> Teachers { get; set; }
    }
}
