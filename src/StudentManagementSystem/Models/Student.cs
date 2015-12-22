using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystem.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        [Required, Display(Name = "Full name")]
        public string FullName { get; set; }
        [Required, Display(Name = "Date of birth"), DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, DataType(DataType.PhoneNumber), Display(Name = "Mobile contact"), RegularExpression("([0-9]+)", ErrorMessage = "Please enter an integer for the Mobile contact field")]
        public string MobileContact { get; set; }
        [Required, Display(Name = "Course")]
        public int CourseId { get; set; }
    }
}
