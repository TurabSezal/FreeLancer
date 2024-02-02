using FreeLancer.App_Classes;
using FreeLancer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FreeLancer.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public void layout()
        {
            ViewBag.User=Context.Baglanti.User.FirstOrDefault(us=>us.email==User.Identity.Name);
        }
        public ActionResult Index()
        {
            layout();
            return View();
        }
        [Authorize]
        public ActionResult MyProfile()
        {
            layout();
            var data=Context.Baglanti.User.FirstOrDefault(us=>us.email== User.Identity.Name);
            if (data.type==0)
            {
                ViewBag.task = Context.Baglanti.Task_Status.Where(ts => ts.userID == data.id).ToList();
            }
            if (data.type==1)
            {
                ViewBag.task=Context.Baglanti.Task.Where(ts=>ts.clientID== data.id).ToList();
            }
            return View(data);
        }
        [Authorize]
        public ActionResult AddTask()
        {
            layout();
            var data = Context.Baglanti.User.FirstOrDefault(us => us.email == User.Identity.Name);
            if (data.type == 0)
            {
                return RedirectToAction("Permission", "Error");
            }
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddTask(Task ts)
        {
            layout();
            var user = Context.Baglanti.User.FirstOrDefault(us => us.email == User.Identity.Name);
            if (user.type == 0)
            {
                return RedirectToAction("Permission", "Error");
            }
            if (user.balance<ts.cost)
            {
                return Json(new {success=false,message= "Your current balance is not sufficient." });
            }
            if (ts.category=="Select")
            {
                return Json(new { success = false, message = "Please select category." });
            }
            if (ts.type=="Select")
            {
                return Json(new { success = false, message = "Please select type." });
            }
            ts.createdat = DateTime.Now;
            ts.clientID= user.id;
            Context.Baglanti.Task.Add(ts);
            Context.Baglanti.SaveChanges();
            return Json(new { success = true});
        }
        public ActionResult Tasks() 
        {
            layout();
            var data=Context.Baglanti.Task.Where(ts=>ts.Task_Status.Count==0).ToList();
            return View(data);
        }
        [Authorize]
        public ActionResult SingleTask(int id)
        {
            layout();
            ViewBag.ID = id;
            var task=Context.Baglanti.Task.FirstOrDefault(cn => cn.id == id);
            ViewBag.client= Context.Baglanti.User.FirstOrDefault(us => us.id == task.clientID);
            return View(Context.Baglanti.Task.FirstOrDefault(ts=>ts.id==id));
        }
        [Authorize]
        public ActionResult AddOffered(int id)
        {
            layout();
            ViewBag.Task=Context.Baglanti.Task.FirstOrDefault(ts=>ts.id== id);
            ViewBag.ID = id;
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddOffered(int id,Offered of)
        {
            var user =Context.Baglanti.User.FirstOrDefault(us=>us.email==User.Identity.Name);
            var exist=Context.Baglanti.Offered.FirstOrDefault(us=>us.userID==user.id);
            if (exist!=null)
            {
                return Json(new { success = false, message = "You have already submitted a bid for this job." });
            }
            of.taskID = id;
            of.userID = user.id;
            Context.Baglanti.Offered.Add(of);
            Context.Baglanti.SaveChanges();
            return Json(new { success = true });
        }
        [Authorize]
        public ActionResult OfferedList(int id)
        {
            var data = Context.Baglanti.User.FirstOrDefault(us => us.email == User.Identity.Name);
            if (data.type == 0)
            {
                return RedirectToAction("Permission", "Error");
            }
            layout();
            var offered = Context.Baglanti.Offered.Where(of => of.taskID == id).ToList();
            return View(offered);
        }
        [Authorize]
        public ActionResult TakeJob(int id)
        {
            var permission=Context.Baglanti.User.FirstOrDefault(us=>us.email==User.Identity.Name);
            var offered=Context.Baglanti.Offered.FirstOrDefault(of => of.id == id);
            var task = Context.Baglanti.Task.FirstOrDefault(ts => ts.id ==offered.taskID);
            var userId = Context.Baglanti.User.FirstOrDefault(us => us.id==offered.userID);
            if (permission.type == 0)
            {
                return RedirectToAction("Permission", "Error");
            }
            Task_Status taskStatus = new Task_Status
            {
                taskID = task.id,
                userID = userId.id,
                status = 1,
            };
            task.cost = offered.offeredCost;
            task.duration= offered.offeredDuration;
            Context.Baglanti.Task_Status.Add(taskStatus);
            var existOffere = Context.Baglanti.Offered.Where(of => of.taskID == offered.taskID).ToList();
            foreach (var ex in existOffere)
            {
                Context.Baglanti.Offered.Remove(ex);
            }
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult UpdateTask(int id)
        {
            var loginUser=Context.Baglanti.User.FirstOrDefault(us=>us.email==User.Identity.Name);
            var taskStatus=Context.Baglanti.Task_Status.FirstOrDefault(ts=>ts.taskID==id);
            var user = Context.Baglanti.User.FirstOrDefault(us=>us.id==taskStatus.userID);
            if (taskStatus != null)
            {
                if (loginUser.type == 1) 
                {
                    taskStatus.Task.User.balance= taskStatus.Task.User.balance -taskStatus.Task.cost;
                    user.balance = user.balance + taskStatus.Task.cost;
                    taskStatus.status = 3;
                }
                if (loginUser.type==0)
                {
                    taskStatus.status = 2;
                }
                
            }
            Context.Baglanti.SaveChanges();
            return RedirectToAction("MyProfile");
        }
    }
}