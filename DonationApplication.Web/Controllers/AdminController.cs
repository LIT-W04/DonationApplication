using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DonationApplication.Data;
using DonationApplication.Web.Models;

namespace DonationApplication.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private User _user;

        public ActionResult Index()
        {
            if (!CheckIfAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoggedInViewModel { User = _user });
        }

        public ActionResult Categories()
        {
            if (!CheckIfAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            var db = new CategoryRepository(Properties.Settings.Default.ConStr);
            return View(new CategoriesViewModel() { Categories = db.GetCategories(true) });
        }

        [HttpPost]
        public ActionResult AddCategory(Category category)
        {
            if (!CheckIfAdmin())
            {
                return RedirectToAction("Index", "Home");
            }
            var db = new CategoryRepository(Properties.Settings.Default.ConStr);
            db.Add(category);
            return RedirectToAction("Categories");
        }

        [HttpPost]
        public void DeleteCategory(int categoryId)
        {
            var db = new CategoryRepository(Properties.Settings.Default.ConStr);
            db.Delete(categoryId);
        }

        [HttpPost]
        public void UpdateCategory(Category category)
        {
            var db = new CategoryRepository(Properties.Settings.Default.ConStr);
            db.Update(category);
        }

        private bool CheckIfAdmin()
        {
            var db = new UserRepository(Properties.Settings.Default.ConStr);
            _user = db.GetUser(User.Identity.Name);
            return _user.IsAdmin;
        }

        public ActionResult PendingApplications()
        {
            if (!CheckIfAdmin())
            {
                return RedirectToAction("Index", "Home");
            }
            var categoryDb = new CategoryRepository(Properties.Settings.Default.ConStr);
            var vm = new CategoriesViewModel
            {
                Categories = categoryDb.GetCategories(false),
            };
            return View(vm);
        }

        public ActionResult GetPendingApplications(int? categoryId)
        {
            var applicationsDb = new ApplicationRespository(Properties.Settings.Default.ConStr);
            var result = applicationsDb.GetPendingApplications(categoryId).Select(a => new
            {
                id = a.Id,
                category = a.Category.Name,
                firstName = a.User.FirstName,
                lastName = a.User.LastName,
                email = a.User.Email,
                amount = a.Amount,
                userId = a.UserId
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Test()
        {
            var applicationsDb = new ApplicationRespository(Properties.Settings.Default.ConStr);
            IEnumerable<Application> result = applicationsDb.GetPendingApplications(null);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void UpdateApplicationStatus(int id, bool isApproved)
        {
            var applicationsDb = new ApplicationRespository(Properties.Settings.Default.ConStr);
            applicationsDb.UpdateApplicationStatus(id, isApproved);
        }

        public ActionResult UserHistory(int userId)
        {
            if (!CheckIfAdmin())
            {
                return RedirectToAction("Index", "Home");
            }
            var userDb = new UserRepository(Properties.Settings.Default.ConStr);
            var user = userDb.GetUser(userId);
            var applicationDb = new ApplicationRespository(Properties.Settings.Default.ConStr);
            var applications = applicationDb.GetUserApplications(userId);

            return View("../Home/History", new HistoryViewModel { User = user, Applications = applications });
        }

    }
}