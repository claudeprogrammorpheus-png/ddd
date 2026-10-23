using System.IO;
using System.Linq;
using System.Web.Mvc;
using ASPNETWebAppMVCStudentApp.Models;
using Microsoft.Reporting.WebForms;

namespace ASPNETWebAppMVCStudentApp.Controllers
{
    public class ReportController : Controller
    {
        private SchoolDBEntities db = new SchoolDBEntities();

        // GET: Report  (form chọn khóa học)
        public ActionResult Index()
        {
            var model = new CourseViewModel
            {
                Courses = new SelectList(db.Courses.OrderBy(c => c.Title), "CourseID", "Title")
            };
            return View(model);
        }

        // POST: Report/Generate
        // action = "preview" -> mở PDF trên trình duyệt (inline, có thể in)
        // action = "export"  -> tải file StudentReport.pdf về máy
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Generate(CourseViewModel model, string action)
        {
            if (!ModelState.IsValid)
            {
                model.Courses = new SelectList(db.Courses.OrderBy(c => c.Title), "CourseID", "Title", model.SelectedCourseID);
                return View("Index", model);
            }

            var course = db.Courses.Find(model.SelectedCourseID);
            if (course == null) return HttpNotFound();

            // Dữ liệu sinh viên của khóa học đã chọn
            var students = db.Enrollments
                .Where(e => e.CourseID == model.SelectedCourseID)
                .Select(e => new StudentReportRow
                {
                    StudentID = e.Student.StudentID,
                    FirstName = e.Student.FirstName,
                    LastName = e.Student.LastName,
                    Grade = e.Grade
                })
                .OrderBy(r => r.LastName)
                .ToList();

            var reportPath = Server.MapPath("~/Reports/StudentReport.rdlc");
            var localReport = new LocalReport { ReportPath = reportPath };
            localReport.DataSources.Add(new ReportDataSource("StudentDataSet", students));
            localReport.SetParameters(new ReportParameter("CourseTitle", course.Title));

            string mimeType, encoding, fileNameExtension;
            string[] streams;
            Warning[] warnings;
            byte[] reportBytes = localReport.Render("PDF", null,
                out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

            var disposition = action == "export" ? "attachment" : "inline";
            Response.AppendHeader("Content-Disposition", disposition + "; filename=StudentReport.pdf");
            return File(reportBytes, "application/pdf");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }

    /// <summary>Một dòng dữ liệu của báo cáo. Tên property khớp với Field trong StudentReport.rdlc.</summary>
    public class StudentReportRow
    {
        public int StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal? Grade { get; set; }
    }
}
