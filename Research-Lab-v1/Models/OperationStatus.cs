using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Research_Lab_v1.Models
{
    public class OperationStatus
    {
        public bool Status { get; set; }
        public string StackTrace { get; set; }
        public string Message { get; set; }
        public int InsertedID { get; set; }
    }
}