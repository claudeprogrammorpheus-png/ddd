using System.Web.Mvc;

namespace ASPNETWebAppMVCStudentApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            // Mọi trang đều yêu cầu đăng nhập, trừ các action có [AllowAnonymous]
            filters.Add(new Filters.LoginRequiredAttribute());
        }
    }
}
