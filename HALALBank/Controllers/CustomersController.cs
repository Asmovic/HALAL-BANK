using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HALALBank.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using HALALBank.Services;

namespace HALALBank.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Customer/
        public ActionResult Index()
        {
            // db.Configuration.ProxyCreationEnabled = false;
            var customers = db.Customers.Include(c => c.User);
            return View(customers.ToList());
        }

        // GET: /Customer/Details/5
        public ActionResult Details(int? id)
        {
            //      db.Configuration.ProxyCreationEnabled = false;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customers customers = db.Customers.Find(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            return View(customers);
        }


        public ActionResult Statement(int id)
        {

            var customerId = HomeController.glb;
          var model = from r in db.Transactions where r.CustomerId == customerId select r;

          
            return View(model);
          


        }

        
        // GET: /Customer/Create
        public ActionResult Create()
        {
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "UserName");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection collection)
        {
            var userId = User.Identity.GetUserId();

            try
            {
                foreach (string _formData in collection)
                {
                    ViewData[_formData] = collection[_formData];
                }

                var service = new CustomerServices(HttpContext.GetOwinContext().Get<ApplicationDbContext>());

                service.CreateCustomer(ViewData["firstName"].ToString(), ViewData["lastName"].ToString(), userId, decimal.Parse(ViewData["balance"].ToString()));


                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        // POST: /Customer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include="id,AccountNumber,firstName,lastName,balance,ApplicationUserId")] Customers customers)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Customers.Add(customers);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "UserName", customers.ApplicationUserId);
        //    return View(customers);
        //}

        // GET: /Customer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customers customers = db.Customers.Find(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "UserName", customers.ApplicationUserId);
            return View(customers);
        }

        // POST: /Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, Customers customer)
        {
            try
            {
                using (ApplicationDbContext dm = new ApplicationDbContext())
                {
                    dm.Entry(customer).State = EntityState.Modified;
                    dm.SaveChanges();
                }
                return RedirectToAction("index");
            }
            catch
            {
                return View();
            }
        }

        //public ActionResult Edit([Bind(Include="id,AccountNumber,firstName,lastName,balance,ApplicationUserId")] Customers customers)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(customers).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "UserName", customers.ApplicationUserId);
        //    return View(customers);
        //}

        // GET: /Customer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customers customers = db.Customers.Find(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            return View(customers);
        }

        // POST: /Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customers customers = db.Customers.Find(id);
            db.Customers.Remove(customers);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //public class CustomersController : Controller
        //{
        //    private ApplicationDbContext db = new ApplicationDbContext();

        //    // GET: /Customers/
        //    public ActionResult Index()
        //    {
        //        var customers = db.Customers.Include(c => c.User);
        //        return View(customers.ToList());
        //    }

        //    // GET: /Customers/Details/5
        //    public ActionResult Details(int? id)
        //    {
        //        if (id == null)
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }
        //        Customers customers = db.Customers.Find(id);
        //        if (customers == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        return View(customers);
        //    }

        //    // GET: /Customers/Create
        //    public ActionResult Create()
        //    {
        //        ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "UserName");
        //        return View();
        //    }

        //    // POST: /Customers/Create
        //    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public ActionResult Create([Bind(Include="id,AccountNumber,firstName,lastName,balance,ApplicationUserId")] Customers customers)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            db.Customers.Add(customers);
        //            db.SaveChanges();
        //            return RedirectToAction("Index");
        //        }

        //        ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "UserName", customers.ApplicationUserId);
        //        return View(customers);
        //    }

        //    // GET: /Customers/Edit/5
        //    public ActionResult Edit(int? id)
        //    {
        //        if (id == null)
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }
        //        Customers customers = db.Customers.Find(id);
        //        if (customers == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "UserName", customers.ApplicationUserId);
        //        return View(customers);
        //    }

        //    // POST: /Customers/Edit/5
        //    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public ActionResult Edit([Bind(Include="id,AccountNumber,firstName,lastName,balance,ApplicationUserId")] Customers customers)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            db.Entry(customers).State = EntityState.Modified;
        //            db.SaveChanges();
        //            return RedirectToAction("Index");
        //        }
        //        ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "UserName", customers.ApplicationUserId);
        //        return View(customers);
        //    }

        //    // GET: /Customers/Delete/5
        //    public ActionResult Delete(int? id)
        //    {
        //        if (id == null)
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }
        //        Customers customers = db.Customers.Find(id);
        //        if (customers == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        return View(customers);
        //    }

        //    // POST: /Customers/Delete/5
        //    [HttpPost, ActionName("Delete")]
        //    [ValidateAntiForgeryToken]
        //    public ActionResult DeleteConfirmed(int id)
        //    {
        //        Customers customers = db.Customers.Find(id);
        //        db.Customers.Remove(customers);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    protected override void Dispose(bool disposing)
        //    {
        //        if (disposing)
        //        {
        //            db.Dispose();
        //        }
        //        base.Dispose(disposing);
        //    }
        //}
    }
}