using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LocalTheatreCompany.Models.ViewModels
{
    //View Model Used for Creating a New Staff Member
    public class CreateStaffViewModel
    {
        //Attributes 

        //User Properties
        [Required]
        [Display(Name = "First Name")]
        public string Firstname { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string Lastname { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }


        [Display(Name = "Email Confirm?")]
        public bool EmailConfirm { get; set; }

        [Display(Name = "Phone Confirm?")]
        public bool PhoneConfirm { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        //To Get the Roles
        public string Role { get; set; }

        public ICollection<SelectListItem> Roles { get; set; }
    }
}