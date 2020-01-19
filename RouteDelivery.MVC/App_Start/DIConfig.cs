using Autofac;
using Autofac.Integration.Mvc;
using RouteDelivery.Data;
using RouteDelivery.Data.Implementations;
using System;
using System.Web.Mvc;
using RouteDelivery.OptimizationEngine;

namespace RouteDelivery
{
    internal class DIConfig
    {
        internal static void Setup()
        {
            var builder = new ContainerBuilder();

            var uof = new UnitOfWork();

            builder.RegisterInstance(uof).As<IUnitOfWork>();
            builder.RegisterInstance(new OptimizationEngine.OptimizationEngine(uof)).As<IOptimizationEngine>();

            // Register your MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }
}