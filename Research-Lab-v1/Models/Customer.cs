﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Research_Lab_v1.Models
{
    public class Customer
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Order> Orders { get; set; }
    }
}