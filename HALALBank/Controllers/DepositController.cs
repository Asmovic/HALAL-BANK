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
    public class DepositController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Deposit/
        public ActionResult Index()
        {
            var transactions = db.Transactions.Include(t => t.customer);
            return View(transactions.ToList());
        }

        // GET: /Deposit/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transactions transactions = db.Transactions.Find(id);
            if (transactions == null)
            {
                return HttpNotFound();
            }
            return View(transactions);
        }

        // GET: /Deposit/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "id", "AccountNumber");
            return View();
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(FormCollection collection)
        //{
        //    var userId = User.Identity.GetUserId();

        //    try
        //    {
        //        foreach (string _formData in collection)
        //        {
        //            ViewData[_formData] = collection[_formData];
        //        }

        //        var service = new CustomerServices(HttpContext.GetOwinContext().Get<ApplicationDbContext>());

        //        service.CreateDeposit(int.Parse(ViewData["Amount"].ToString()));

        //        Transactions trs = new Transactions();

        //        var services = new CustomerServices(db);
        //        services.UpdateCustomer(trs.CustomerId);


        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // POST: /Deposit/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Transactions transactions)
        {


            transactions.CustomerId = HomeController.glb;
            if (ModelState.IsValid)
            {
                //  transactions.CustomerId = HomeController.glb;
                db.Transactions.Add(transactions);
                db.SaveChanges();

                var service = new CustomerServices(db);
                service.UpdateCustomer(transactions.CustomerId);
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "id", "AccountNumber", transactions.CustomerId);
            return View(transactions);
        }

        // GET: /Deposit/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transactions transactions = db.Transactions.Find(id);
            if (transactions == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "id", "AccountNumber", transactions.CustomerId);
            return View(transactions);
        }

        // POST: /Deposit/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Amount,CustomerId")] Transactions transactions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transactions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "id", "AccountNumber", transactions.CustomerId);
            return View(transactions);
        }

        // GET: /Deposit/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transactions transactions = db.Transactions.Find(id);
            if (transactions == null)
            {
                return HttpNotFound();
            }
            return View(transactions);
        }

        // POST: /Deposit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transactions transactions = db.Transactions.Find(id);
            db.Transactions.Remove(transactions);
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
    }
}
