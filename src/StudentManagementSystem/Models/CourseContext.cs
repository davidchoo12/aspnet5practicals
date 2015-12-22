using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystem.Models
{
    public class CourseContext
    {
        private string _connectionString;
        public CourseContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        #region Retrieve
        public List<Course> GetCourses()
        {
            DataSet ds = new DataSet();
            using (MySqlConnection cn = new MySqlConnection(_connectionString))
            using (MySqlCommand cmd = new MySqlCommand())
            using(MySqlDataAdapter da = new MySqlDataAdapter())
            {
                cmd.Connection = cn;
                cmd.CommandText = "SELECT * FROM course;";
                da.SelectCommand = cmd;
                try
                {
                    cn.Open();
                    da.Fill(ds, "course");
                }
                catch (MySqlException e)
                {
                    
                }
                finally
                {
                    cn.Close();
                }
            }
            List<Course> courses = ds.Tables["course"].AsEnumerable().Select(row => new Course()
            {
                CourseId = row.Field<int>("CourseId"),
                CourseName = row.Field<string>("CourseName"),
                CourseAbbreviation = row.Field<string>("CourseAbbreviation")
            }).ToList();
            return courses;
        }
        #endregion
    }
}
