using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using StudentManagementSystem.Models;
using Microsoft.Extensions.OptionsModel;
using StudentManagementSystem.ViewModels.Student;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentManagementSystem.Controllers
{
    public class StudentController : Controller
    {
        private IOptions<MyOptions> _options;
        public StudentController(IOptions<MyOptions> options)
        {
            _options = options;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            List<StudentViewModel> studentVMs = new List<StudentViewModel>();
            try
            {
                List<Student> students = new StudentContext(_options.Value.ConnectionString).GetStudents();
                List<Course> courses = new CourseContext(_options.Value.ConnectionString).GetCourses();
                studentVMs = JoinStudentsAndCourses(students, courses);
            }
            catch (Exception e)
            {
                ViewData["divMessage"] = "Unable to retrieve data. Please contact the administrator if the problem persists";
            }
            return View(studentVMs);
        }
        [HttpPost]
        public IActionResult Index(string search)
        {
            ViewData["students"] = new List<Student>();
            try
            {
                List<Student> students = new StudentContext(_options.Value.ConnectionString).GetStudent(search);
                ViewData["students"] = students;
                ViewData["divMessage"] = string.Format("\"{0}\" returned {1} results", search, students.Count);
            }
            catch (Exception e)
            {
                ViewData["divMessage"] = "Unable to retrieve data. Please contact the administrator if the problem persists";
            }
            return View();
        }
        private List<StudentViewModel> JoinStudentsAndCourses(List<Student> students, List<Course> courses)
        {
            List<StudentViewModel> studentVMs = students.AsEnumerable().Join(
                courses,
                student => student.CourseId,
                course => course.CourseId,
                (student, course) =>
                {
                    StudentViewModel studentVM = new StudentViewModel()
                    {
                        StudentId = student.StudentId,
                        FullName = student.FullName,
                        DateOfBirth = student.DateOfBirth,
                        Email = student.Email,
                        MobileContact = student.MobileContact,
                        CourseAbbreviation = course.CourseAbbreviation
                    };
                    return studentVM;
                }).ToList();
            return studentVMs;
        }
        [HttpPost]
        public IActionResult Delete(int[] checkedId)
        {
            StudentContext sc = new StudentContext(_options.Value.ConnectionString);
            bool success = false;
            int deleteCount = 0;
            foreach (int id in checkedId)
            {
                success = sc.DeleteOneStudent(id);
                if (success)
                {
                    deleteCount++;
                    continue;
                }
                else
                    break;
            }
            /*ViewData cannot be transfered through a redirect, so we can use TempData, but we have to configure the session middleware in Startup.cs.
            You are always welcomed to find out more on using sessions*/
            
            /*
            if (success)
                TempData["divMessage"] = "Successfully deleted " + deleteCount + " record(s).";
            else
                TempData["divMessage"] = "Failed to delete some record(s). Please contact the administrator if the problem persists.";
            */
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Add()
        {
            ViewData["courses"] = new CourseContext(_options.Value.ConnectionString).GetCourses();
            ViewData["divMessage"] = "";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Student student)
        {
            ViewData["courses"] = new CourseContext(_options.Value.ConnectionString).GetCourses();
            string messageToUser = "";
            StudentContext context = new StudentContext(_options.Value.ConnectionString);
            bool success = false;
            try
            {
                success = context.AddStudent(student);
            }
            catch (Exception e)
            {
                messageToUser = e.Message;
            }
            if (success)
                ViewData["divMessage"] = "Successfully added " + student.FullName;
            else
                ViewData["divMessage"] = "Failed to add " + student.FullName + ". Reason: " + messageToUser;
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewData["courses"] = new CourseContext(_options.Value.ConnectionString).GetCourses();
            ViewData["divMessage"] = "Received record id: " + id;
            Student student = new StudentContext(_options.Value.ConnectionString).GetOneStudent(id);
            return View(student);
        }

        [HttpPost]
        public IActionResult Edit(Student student)
        {
            ViewData["courses"] = new CourseContext(_options.Value.ConnectionString).GetCourses();
            bool success = false;
            string exceptionMessage = "";
            try
            {
                success = new StudentContext(_options.Value.ConnectionString).UpdateOneStudent(student);
            }
            catch (Exception e)
            {
                exceptionMessage = e.Message;
            }
            if (success)
            {
                //If update is successful, redirect the user to the Index page
                return RedirectToAction("Index");
            }
            else
            {
                ViewData["divMessage"] = "Record is not successfully saved. Reason: " + exceptionMessage;
                return View(student);
            }
        }
    }
}
