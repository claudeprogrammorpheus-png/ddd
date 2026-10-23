using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ASPNETWebAppMVCStudentApp.Models
{
    /// <summary>Form chọn khóa học cho báo cáo (Exercise 4).</summary>
    public class CourseViewModel
    {
        [Display(Name = "Course")]
        [Required(ErrorMessage = "Please select a course")]
        public int SelectedCourseID { get; set; }

        public SelectList Courses { get; set; }
    }

    /// <summary>Form đăng nhập (Exercise 3).</summary>
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }

    /// <summary>Form đặt lại mật khẩu khi đăng nhập thất bại (Exercise 3).</summary>
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "New password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }
    }
}
