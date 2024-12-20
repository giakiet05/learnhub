﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
    public class Grade : IAdminId
    {
        public Guid Id { get; set; }
        public string OriginalId { get; set; }
        public int Number { get; set; }

        public string? Name { get; set; }
        public Guid? AdminId { get; set; }
        public Admin Admin { get; set; }
        //Navigation Properties
        public ICollection<Classroom> Classrooms { get; set; }
        public ICollection<Subject> Subjects { get; set; }

       
    }

}
