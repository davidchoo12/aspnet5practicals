using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystem.ViewModels.Student
{
    public class StudentViewModel
    {
        public int StudentId { get; set; }
        [Display(Name = "Full name")]
        public string FullName { get; set; }
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        [Display(Name = "Mobile Contact")]
        public string MobileContact { get; set; }
        [Display(Name = "Course")]
        public string CourseAbbreviation { get; set; }
    }
}
