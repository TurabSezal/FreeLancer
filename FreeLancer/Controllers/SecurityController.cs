using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FreeLancer.Models;

namespace FreeLancer.Controllers
{
    public class SecurityController : Controller
    {
        FreeLancerEntities db = new FreeLancerEntities();
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Save(User us)
        {
            using (var context = new FreeLancerEntities())
            {
                var exist=context.User.FirstOrDefault(ex=>ex.email== us.email);
                if (exist!=null)
                {
                    return RedirectToAction("Register");
                }
                context.User.Add(us);
                us.balance = 0;
                context.SaveChanges();
            }

            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User us)
        {
            var bilgiler = db.User.FirstOrDefault(x => x.email == us.email && x.password == us.password);
            if (bilgiler != null)
            {
                FormsAuthentication.SetAuthCookie(bilgiler.email, true);
                return RedirectToAction("Index","Home");
            }
            else
            {
                return View();
            }

        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}

