using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DonationApplication.Data;
using DonationApplication.Web.Models;

namespace DonationApplication.Web.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult CheckEmailExists(string email)
        {
            var db = new UserRepository(Properties.Settings.Default.ConStr);
            return Json(new { exists = db.EmailExists(email) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Register(string firstName, string lastName, string email, string password)
        {
            var db = new UserRepository(Properties.Settings.Default.ConStr);
            db.AddUser(firstName, lastName, email, password, false);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Login()
        {
            var vm = new LoginViewModel
            {
                Email = TempData["email"] as string,
                ErrorMessage = TempData["error"] as string
            };
            return View(vm);
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var db = new UserRepository(Properties.Settings.Default.ConStr);
            var user = db.Login(email, password);
            if (user == null)
            {
                TempData["error"] = "Invalid login, please try again";
                TempData["email"] = email;
                return RedirectToAction("Login");
            }

            FormsAuthentication.SetAuthCookie(email, true);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}