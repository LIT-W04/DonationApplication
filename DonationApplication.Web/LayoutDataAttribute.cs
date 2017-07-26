using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DonationApplication.Data;

namespace DonationApplication.Web
{
    public class LayoutDataAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var db = new UserRepository(Properties.Settings.Default.ConStr);
                var user = db.GetUser(filterContext.HttpContext.User.Identity.Name);
                filterContext.Controller.ViewBag.User = user;
            }
            base.OnActionExecuting(filterContext);

            //in Global.asax:
            //  GlobalFilters.Filters.Add(new LayoutDataAttribute());
        }
    }
}