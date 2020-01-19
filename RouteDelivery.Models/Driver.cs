using RouteDelivery.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RouteDelivery.Models
{
    public class Driver: IEntity
    {
        [Display(Name = "Driver No")]
        public int ID { get; set; }
        [Display(Name = "Driver Name")]
        public string DriverName { get; set; }
        [Display(Name = "Transport Type")]
        public Type.TransportType TransportType { get; set; }
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }
        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }
        [Display(Name = "Start Location")]
        public string StartLocation { get; set; }
    }
}