using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LocalTheatreCompany.Models.ViewModels
{
    //View Model Used for Editing a Customer User
    public class EditCustomerViewModel
    {
        //Attributes

        //User Properties
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        [Display(Name = "Postal Code")]
        public string PostCode { get; set; }

        [Required]
        [Display(Name = "Mobile Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\d{11})$", ErrorMessage = "Please Enter a 11 Digit Phone Number!")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Is Suspended?")]
        public bool IsSuspended { get; set; }
    }
}