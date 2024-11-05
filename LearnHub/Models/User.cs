using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
  public class User : DomainObject
    {
       
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }


        //Navigation Properties
       public ICollection<Notification> Notifications { get; set; }
    }
}
