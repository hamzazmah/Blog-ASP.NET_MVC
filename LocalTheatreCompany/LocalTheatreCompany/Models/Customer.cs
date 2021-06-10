using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocalTheatreCompany.Models
{
    public class Customer : User
    {

        //Customer Specific Attribute
        
        //to Suspend or Un-suspend them from Commenting
        public bool IsSuspended { get; set; }
    }
}