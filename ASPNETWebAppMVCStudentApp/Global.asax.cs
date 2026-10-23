using System;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Routing;
using ASPNETWebAppMVCStudentApp.Models;

namespace ASPNETWebAppMVCStudentApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Database đã được tạo bằng SchoolDB.sql, không để EF tự tạo/migrate
            Database.SetInitializer<SchoolDBEntities>(null);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
