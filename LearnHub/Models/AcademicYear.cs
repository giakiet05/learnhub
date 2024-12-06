using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
    public class AcademicYear : IAdminId
    {
        public Guid Id { get; set; } //khóa chính thực sự
        public string OriginalId { get; set; } //Id do người dùng nhập
        public string? Name { get; set; }

        public int? StartYear { get; set; }
        public Guid? AdminId { get; set; }
        public Admin Admin { get; set; }
        //Navigation Properties
        public ICollection<SemesterResult> SemesterResults { get; set; }
        public ICollection<Classroom> Classrooms { get; set; }
        public ICollection<Score> SubjectResults { get; set; }
     
    }
}
