using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureADAuth.Entities
{
    public class SalesContext : DbContext
    {

        public SalesContext(DbContextOptions<SalesContext> options) : base(options)

        {

        }

        public DbSet<Customer> customers { get; set; }
    }
}
