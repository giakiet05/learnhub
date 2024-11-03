using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
    public class Student
    {
        [Key, ForeignKey("User")]
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string? Address { get; set; }
        public DateTime? BirthDay { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Ethnicity { get; set; }
        public string? Religion { get; set; }
        public string? FatherName { get; set; }
        public string? MotherName { get; set; }
        public string? FatherPhone { get; set; }
        public string? MotherPhone { get; set; }
         //Navigation Properties
        public User User { get; set; }
        public StudentPlacement StudentPlacement { get; set; }
        public ICollection<SubjectResult> SubjectResults { get; set; }
        public ICollection<YearResult> YearResults { get; set; }
    }
}
