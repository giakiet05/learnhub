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
        public int Username { get; set; }
    }
}
