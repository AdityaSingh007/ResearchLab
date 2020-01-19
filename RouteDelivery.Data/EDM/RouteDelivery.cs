namespace RouteDelivery.Data.EDM
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class RouteDelivery : DbContext
    {
        // Your context has been configured to use a 'RouteDelivery' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'RouteDelivery.Data.EDM.RouteDelivery' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'RouteDelivery' 
        // connection string in the application configuration file.
        public RouteDelivery()
            : base("name=RouteDelivery")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Delivery> Deliveries { get; set; }
        public virtual DbSet<DeliverySchedule> DeliverySchedules { get; set; }
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<OptimizationRequest> OptimizationRequests { get; set; }
    }
}