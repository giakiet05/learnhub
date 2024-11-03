﻿using System;
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
        public string Password { get; set; }
        public string Role { get; set; }
       

        //Navigation Properties
        public Teacher Teacher { get; set; }
        public Student Student { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
