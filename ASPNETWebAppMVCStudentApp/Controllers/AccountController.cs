using System.Linq;
using System.Web.Mvc;
using ASPNETWebAppMVCStudentApp.Helpers;
using ASPNETWebAppMVCStudentApp.Models;

namespace ASPNETWebAppMVCStudentApp.Controllers
{
    public class AccountController : Controller
    {
        private SchoolDBEntities db = new SchoolDBEntities();

        // GET: Account/Login  (màn hình đầu tiên của ứng dụng)
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Index", "Students");
            }
            return View(new LoginViewModel());
        }

        // POST: Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = db.Users.FirstOrDefault(u => u.Username == model.Username);
            if (user != null && PasswordHelper.Matches(user.PasswordHash, model.Password))
            {
                Session["UserID"] = user.UserID;
                Session["UserName"] = user.FullName;
                return RedirectToAction("Index", "Students");
            }

            // Đăng nhập thất bại: báo lỗi và gợi ý đặt lại mật khẩu
            ModelState.AddModelError("", "Invalid username or password.");
            ViewBag.ShowResetLink = true;
            return View(model);
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        // GET: Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            return View(new ResetPasswordViewModel());
        }

        // POST: Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = db.Users.FirstOrDefault(u => u.Username == model.Username && u.Email == model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "No account matches this username and email.");
                return View(model);
            }

            user.PasswordHash = PasswordHelper.Hash(model.NewPassword);
            db.SaveChanges();

            TempData["Message"] = "Password has been reset. Please log in with your new password.";
            return RedirectToAction("Login");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
