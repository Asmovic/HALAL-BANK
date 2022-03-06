using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HALALBank.Models;
using HALALBank.Services;

namespace HALALBank.Controllers
{
    public class WithdrawalController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Withdrawal/
        public ActionResult Index()
        {
            var transactions = db.Transactions.Include(t => t.customer);
            return View(transactions.ToList());
        }

        // GET: /Withdrawal/Details/5
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

        // GET: /Withdrawal/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "id", "AccountNumber");
            return View();
        }

        // POST: /Withdrawal/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(Transactions transactions)
        {
            transactions.CustomerId = HomeController.glb;
            var customer = db.Customers.Find(transactions.CustomerId);

            if (customer.balance < transactions.Amount)
            {
                ModelState.AddModelError("", "You have insuficient funds!");
            }

            if (ModelState.IsValid)
            {
               


                transactions.Amount = -transactions.Amount;
                db.Transactions.Add(transactions);
                db.SaveChanges();

                var service = new CustomerServices(db);
                service.UpdateCustomer(transactions.CustomerId);
                return RedirectToAction("Index","Home");
            }

          //  ViewBag.CustomerId = new SelectList(db.Customers, "id", "AccountNumber", transactions.CustomerId);
            //return View(transactions);
            return View();
        }

        public ActionResult QuickCash()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "id", "AccountNumber");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult QuickCash(Transactions transactions)
        {
            transactions.CustomerId = HomeController.glb;
            var customer = db.Customers.Find(transactions.CustomerId);
            decimal amt = 1000;
            if (customer.balance < amt)
            {
                ModelState.AddModelError("", "You have insuficient funds!");
            }

            if (ModelState.IsValid)
            {
                
                transactions.Amount = -amt;
                db.Transactions.Add(transactions);
                db.SaveChanges();

                var service = new CustomerServices(db);
                service.UpdateCustomer(transactions.CustomerId);
                return RedirectToAction("Index", "Home");
            }

            //  ViewBag.CustomerId = new SelectList(db.Customers, "id", "AccountNumber", transactions.CustomerId);
            //return View(transactions);
            return View();
        }

        //public ActionResult Create([Bind(Include="Id,Amount,CustomerId")] Transactions transactions)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Transactions.Add(transactions);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.CustomerId = new SelectList(db.Customers, "id", "AccountNumber", transactions.CustomerId);
        //    return View(transactions);
        //}

        // GET: /Withdrawal/Edit/5
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

        // POST: /Withdrawal/Edit/5
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

        // GET: /Withdrawal/Delete/5
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

        // POST: /Withdrawal/Delete/5
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
