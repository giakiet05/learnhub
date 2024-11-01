using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
    public class Student
    {
        //fk username
        public string FatherName { get; }
        public string MotherName { get;  }
        public string FatherPhone { get; }
        public string MotherPhone { get; }

    }
}
