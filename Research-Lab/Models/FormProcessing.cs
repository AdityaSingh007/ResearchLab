using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Research_Lab.Models
{
    public class FormProcessing
    {
        public string fullname { get; set; }
        public string description { get; set; }
        public string employmentstatus { get; set; }
        public string quirk { get; set; }
        public string indentation { get; set; }
        public string fast { get; set; }
        public string git { get; set; }
        public string bonus { get; set; }
    }

    public class TestData
    {
        public string one { get; set; }
        public string Two { get; set; }

        public string Three { get; set; }

        public string[] Four { get; set; }
    }
}