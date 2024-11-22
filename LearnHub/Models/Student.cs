using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
    [Table("Students")]
    public class Student : User
    {
        
        public string? FullName { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public DateTime? Birthday { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Ethnicity { get; set; }
        public string? Religion { get; set; }
        public string? FatherName { get; set; }
        public string? MotherName { get; set; }
        public string? FatherPhone { get; set; }
        public string? MotherPhone { get; set; }
        //Navigation Properties
        public ICollection<StudentPlacement> StudentPlacements { get; set; }
        public ICollection<Score> SubjectResults { get; set; }
        public ICollection<YearResult> YearResults { get; set; }
    }
}
