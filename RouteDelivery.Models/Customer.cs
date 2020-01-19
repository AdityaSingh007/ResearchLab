using RouteDelivery.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RouteDelivery.Models
{
    public class Customer: IEntity
    {
        
        [Display (Name = "Customer Name")]
        public string CustomerName { get; set; }
        [Display(Name = "Customer No")]
        public int ID { get; set; }
        [Display(Name = "Customer Location")]
        public string CustomerLocation { get; set; }
    }
}