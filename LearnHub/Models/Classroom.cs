﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Models
{
    public class Classroom : IAdminId
    {
        public Guid Id { get; set; }
        public string OriginalId { get; set; }
        public string Name { get; set; }
        public int? Capacity { get; set; }
        public Guid? GradeId { get; set; }
        public Guid? TeacherInChargeId { get; set; }

        [ForeignKey("AcademicYear")]
        public Guid? YearId { get; set; }
        public Guid? AdminId { get; set; }
        public Admin Admin { get; set; }
        //Navigation Properties
        public Grade Grade { get; set; }
        public Teacher TeacherInCharge { get; set; }
        public AcademicYear AcademicYear { get; set; }
        public ICollection<StudentPlacement> StudentPlacements { get; set; }
        public ICollection<TeachingAssignment> TeachingAssignments { get; set; }
        public ICollection<ExamSchedule> ExamSchedules { get; set; }
     

    }
}
