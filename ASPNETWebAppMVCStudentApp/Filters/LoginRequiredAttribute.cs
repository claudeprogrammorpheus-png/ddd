using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ASPNETWebAppMVCStudentApp.Filters
{
    /// <summary>
    /// Chặn truy cập khi chưa đăng nhập (Exercise 3).
    /// Đặt [AllowAnonymous] lên action/controller nếu muốn bỏ qua kiểm tra.
    /// </summary>
    public class LoginRequiredAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool allowAnonymous =
                filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
                filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);

            if (allowAnonymous) return;

            if (filterContext.HttpContext.Session["UserID"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                    new { controller = "Account", action = "Login" }));
            }
        }
    }
}
