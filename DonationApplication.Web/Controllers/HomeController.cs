using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DonationApplication.Data;
using DonationApplication.Web.Models;

namespace DonationApplication.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var db = new UserRepository(Properties.Settings.Default.ConStr);
                var user = db.GetUser(User.Identity.Name);
                if (user.IsAdmin)
                {
                    return RedirectToAction("Index", "Admin");
                }

                return View("LoggedIn", new LoggedInViewModel { User = user });
            }
            return View();
        }

        [Authorize]
        public ActionResult Application()
        {
            var db = new CategoryRepository(Properties.Settings.Default.ConStr);
            return View(new ApplicationViewModel { Categories = db.GetCategories(false) });
        }

        [HttpPost]
        [Authorize]
        public ActionResult Application(int categoryId, decimal amount, string description)
        {
            var userDb = new UserRepository(Properties.Settings.Default.ConStr);
            var user = userDb.GetUser(User.Identity.Name);
            var application = new Application
            {
                Amount = amount,
                CategoryId = categoryId,
                Description = description,
                CreatedOn = DateTime.Now,
                UserId = user.Id
            };
            var applicationDb = new ApplicationRespository(Properties.Settings.Default.ConStr);
            applicationDb.AddApplication(application);
            return RedirectToAction("History", "Home");
        }

        [Authorize]
        public ActionResult History()
        {
            var userDb = new UserRepository(Properties.Settings.Default.ConStr);
            var user = userDb.GetUser(User.Identity.Name);
            var applicationDb = new ApplicationRespository(Properties.Settings.Default.ConStr);
            var applications = applicationDb.GetUserApplications(user.Id);
            return View(new HistoryViewModel { User = user, Applications = applications });
        }
    }
}