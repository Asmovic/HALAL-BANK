namespace HALALBank.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using HALALBank.Services;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using HALALBank.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<HALALBank.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;

         //   ContextKey = "HALALBank.Models.ApplicationDbContext";
        }

        protected override void Seed(HALALBank.Models.ApplicationDbContext context)
        {
            var UserStore = new UserStore<ApplicationUser>(context);
            var UserManager = new UserManager<ApplicationUser>(UserStore);

            if (!context.Users.Any(t => t.UserName == "admin@halalbank.com"))
            {
                var user = new ApplicationUser { UserName = "admin@halalbank.com", Email = "admin@halalbank.com" };
                UserManager.Create(user, "passW0rd!");
                var service = new CustomerServices(context);
                service.CreateCustomer("admin", "user", user.Id, 0);
                context.Roles.AddOrUpdate(a => a.Name, new IdentityRole { Name = "Admin" });
                context.SaveChanges();
                UserManager.AddToRole(user.Id, "Admin");
                

            }

            //context.Transactions.Add(new Transactions { Amount = 75, CustomerId = 2 });
            //context.Transactions.Add(new Transactions { Amount = -25, CustomerId = 2 });
            //context.Transactions.Add(new Transactions { Amount = 100000, CustomerId = 2 });
            //context.Transactions.Add(new Transactions { Amount = 19.99m, CustomerId = 2 });
            //context.Transactions.Add(new Transactions { Amount = 64.40m, CustomerId = 2 });
            //context.Transactions.Add(new Transactions { Amount = 100, CustomerId = 2 });
            //context.Transactions.Add(new Transactions { Amount = -300, CustomerId = 2 });
            //context.Transactions.Add(new Transactions { Amount = 255.75m, CustomerId = 2 });
            //context.Transactions.Add(new Transactions { Amount = 198, CustomerId = 2 });
            //context.Transactions.Add(new Transactions { Amount = 2, CustomerId = 2 });
            //context.Transactions.Add(new Transactions { Amount = 50, CustomerId = 2 });
            //context.Transactions.Add(new Transactions { Amount = -1.50m, CustomerId = 2 });
            //context.Transactions.Add(new Transactions { Amount = 6100, CustomerId = 2 });
            //context.Transactions.Add(new Transactions { Amount = 164.84m, CustomerId = 3 });
            //context.Transactions.Add(new Transactions { Amount = .01m, CustomerId = 3 });

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
