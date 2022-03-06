using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HALALBank.Models;
using HALALBank.Controllers;

namespace HALALBank.Services
{
    public class CustomerServices
    {
        private ApplicationDbContext db;

        public CustomerServices(ApplicationDbContext dbContext)
        {
            db = dbContext;
        }
        public void CreateCustomer(string firstName, string lastName, string userId, decimal initialBalance)
        {

            var accountNumber = (db.Customers.Count()).ToString().PadLeft(10, '0');
            var customer = new Customers { firstName = firstName, lastName = lastName, AccountNumber = accountNumber, balance = initialBalance, ApplicationUserId = userId };
            db.Customers.Add(customer);

            db.SaveChanges();
        }

        public void CreateDeposit(int Amount)
        {

            int id = HomeController.glb;
            var deposit = new Transactions { Amount = Amount, CustomerId = id };
            db.Transactions.Add(deposit);

            db.SaveChanges();

        }

        public void UpdateCustomer(int customerId)
        {

            var customer = db.Customers.Where(c => c.id == customerId).First();
            customer.balance = db.Transactions.Where(c => c.CustomerId == customerId).Sum(c => c.Amount);
            db.SaveChanges();
        }

        public void CreateCheckingBalance(string firstName, string lastName, string userId, decimal initialBalance)
        {

            var accountNumber = (1234567 + db.Customers.Count()).ToString().PadLeft(10, '0');
            var customer = new Customers { firstName = firstName, lastName = lastName, AccountNumber = accountNumber, balance = initialBalance, ApplicationUserId = userId };
            db.Customers.Add(customer);

            db.SaveChanges();
        }
    }
}