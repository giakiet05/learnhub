using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
  public class User
    {
        [Key]
        public string Username { get; set; }

        public string FullName { get; set; }
        public string Address { get; set; }
        public DateTime BirthDay { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Ethnicity { get; set; }
        public string Religion { get; set; }
        public string CitizenID { get; set; }

        //Navigation Properties
        public Teacher Teacher { get; set; }
        public Student Student { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
