using FreeLancer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FreeLancer.App_Classes;
namespace FreeLancer.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public void layout()
        {
            ViewBag.User = Context.Baglanti.User.FirstOrDefault(us => us.email == User.Identity.Name);

        }
        public ActionResult Permission()
        {
            layout();
            return View();
        }
    }
}