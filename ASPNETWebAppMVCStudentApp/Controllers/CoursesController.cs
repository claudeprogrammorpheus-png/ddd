using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ASPNETWebAppMVCStudentApp.Models;

namespace ASPNETWebAppMVCStudentApp.Controllers
{
    public class CoursesController : Controller
    {
        private SchoolDBEntities db = new SchoolDBEntities();

        private void LoadDepartments(object selected = null)
        {
            ViewBag.DepartmentID = new SelectList(db.Departments.OrderBy(d => d.Name), "DepartmentID", "Name", selected);
        }

        // GET: Courses
        public ActionResult Index()
        {
            return View(db.Courses.Include(c => c.Department).OrderBy(c => c.CourseID).ToList());
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Course course = db.Courses.Include(c => c.Department).FirstOrDefault(c => c.CourseID == id);
            if (course == null) return HttpNotFound();
            return View(course);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            LoadDepartments();
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseID,Title,Credits,DepartmentID")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            LoadDepartments(course.DepartmentID);
            return View(course);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Course course = db.Courses.Find(id);
            if (course == null) return HttpNotFound();
            LoadDepartments(course.DepartmentID);
            return View(course);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseID,Title,Credits,DepartmentID")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            LoadDepartments(course.DepartmentID);
            return View(course);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Course course = db.Courses.Include(c => c.Department).FirstOrDefault(c => c.CourseID == id);
            if (course == null) return HttpNotFound();
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            try
            {
                db.Courses.Remove(course);
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Cannot delete this course because it has enrollments.");
                return View(course);
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
