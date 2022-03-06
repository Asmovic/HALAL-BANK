using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HALALBank.Models;
using HALALBank.Services;

namespace HALALBank.Controllers
{
    public class TransferController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
         //GET: /Transfer/

        //public ActionResult Index(int CustomerId)
        //{
        //    return View();
        //}
        public ActionResult Transfer(int CustomerId)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Transfer(Transfer transfer)
        {
            var sourceCustomer = db.Customers.Find(transfer.CustomerId);

            if(sourceCustomer.balance < transfer.Amount)
            {
                ModelState.AddModelError("Amount", "You have less than the amount you want to transfer in your Account");
            }

            var destAcct = db.Customers.Where(c => c.AccountNumber == transfer.DestinationAccountNo).FirstOrDefault();

            if(destAcct == null)
            {
                ModelState.AddModelError("DestinationAccountNo", "Invalid Account Number");
            }

            if(ModelState.IsValid)
            {
                db.Transactions.Add(new Transactions { Amount = -transfer.Amount, CustomerId = transfer.CustomerId });
                db.Transactions.Add(new Transactions { Amount = transfer.Amount, CustomerId = destAcct.id });

                db.SaveChanges();

                var service = new CustomerServices(db);

                service.UpdateCustomer(transfer.CustomerId);
                service.UpdateCustomer(destAcct.id);

                return PartialView("_TransferSuccess", transfer);
            }
            return PartialView("_TransferForm");
        }
	}
}