using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Net.Http;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using HALALBank.Models;

namespace HALALBank.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public static int glb;
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
          var userId = User.Identity.GetUserId();
          
          if (userId != null)
          {
              var customer = db.Customers.Where(c => c.ApplicationUserId == userId).First().id;
              glb = customer;
              ViewBag.CustomerId = customer;

              //var manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

              //var user = manager.FindById(userId);

              //ViewBag.Pin = user.Pin;
          }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Having trouble? Contact Us!";

            return View();
        }


        [HttpPost]
        public ActionResult Contact(string message)
        {

            ViewBag.Message = "Thanks, we got your Message!";
            return PartialView("_ContactPartial");
        }
    }
}