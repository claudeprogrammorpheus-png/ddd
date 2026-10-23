using System.Data.Entity;

namespace ASPNETWebAppMVCStudentApp.Models
{
    /// <summary>
    /// DbContext kết nối tới SchoolDB (Database First).
    /// Chuỗi kết nối "SchoolDBEntities" nằm trong Web.config.
    /// </summary>
    public class SchoolDBEntities : DbContext
    {
        public SchoolDBEntities() : base("name=SchoolDBEntities")
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
