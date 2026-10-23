using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ASPNETWebAppMVCStudentApp.Models;

namespace ASPNETWebAppMVCStudentApp.Controllers
{
    public class EnrollmentsController : Controller
    {
        private SchoolDBEntities db = new SchoolDBEntities();

        private void LoadLists(int? courseId = null, int? studentId = null)
        {
            ViewBag.CourseID = new SelectList(db.Courses.OrderBy(c => c.Title), "CourseID", "Title", courseId);
            ViewBag.StudentID = new SelectList(
                db.Students.OrderBy(s => s.LastName).ToList().Select(s => new
                {
                    s.StudentID,
                    FullName = s.FirstName + " " + s.LastName
                }),
                "StudentID", "FullName", studentId);
        }

        // GET: Enrollments
        public ActionResult Index()
        {
            var enrollments = db.Enrollments.Include(e => e.Course).Include(e => e.Student)
                                .OrderBy(e => e.EnrollmentID).ToList();
            return View(enrollments);
        }

        // GET: Enrollments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Enrollment enrollment = db.Enrollments.Include(e => e.Course).Include(e => e.Student)
                                      .FirstOrDefault(e => e.EnrollmentID == id);
            if (enrollment == null) return HttpNotFound();
            return View(enrollment);
        }

        // GET: Enrollments/Create
        public ActionResult Create()
        {
            LoadLists();
            return View();
        }

        // POST: Enrollments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EnrollmentID,CourseID,StudentID,Grade")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Enrollments.Add(enrollment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            LoadLists(enrollment.CourseID, enrollment.StudentID);
            return View(enrollment);
        }

        // GET: Enrollments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null) return HttpNotFound();
            LoadLists(enrollment.CourseID, enrollment.StudentID);
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EnrollmentID,CourseID,StudentID,Grade")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enrollment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            LoadLists(enrollment.CourseID, enrollment.StudentID);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Enrollment enrollment = db.Enrollments.Include(e => e.Course).Include(e => e.Student)
                                      .FirstOrDefault(e => e.EnrollmentID == id);
            if (enrollment == null) return HttpNotFound();
            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Enrollment enrollment = db.Enrollments.Find(id);
            db.Enrollments.Remove(enrollment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
