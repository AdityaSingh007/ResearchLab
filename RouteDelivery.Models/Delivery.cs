using RouteDelivery.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RouteDelivery.Models
{
    public class Delivery: IEntity
    {
        [Display(Name = "Customer No")]
        public int CustomerID { get; set; }
        [Display(Name = "Package No")]
        public int ID { get; set; }
        [Display(Name = "Transport Type")]
        public Type.TransportType TransportType { get; set; }
    }
}