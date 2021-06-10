using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using LocalTheatreCompany.Models;
using LocalTheatreCompany.Models.ViewModels;

namespace LocalTheatreCompany.Controllers
{
    //Customers Can Edit their own personal Details
    [Authorize(Roles = "Customer")]
    public class CustomerController : AccountController
    {
        //Instance of Jewellery Db Context
        private LocalTheatreCompanyDbContext db = new LocalTheatreCompanyDbContext();

        public CustomerController() : base()
        {

        }

        public CustomerController(ApplicationUserManager userManager, ApplicationSignInManager signInManager) : base(userManager, signInManager)
        {

        }

        //For Customers to Edit their own Details
        [HttpGet]
        public ActionResult EditCustomer(string id)
        {
            //Check if the Id is Valid
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Find the Customer by there ID
            Customer customer = db.Users.Find(id) as Customer;

            //Check if Customer is Valid
            if (customer == null)
            {
                return HttpNotFound();
            }

            //Send Customer Details to the View
            return View(new Customer
            {
                Street = customer.Street,
                City = customer.City,
                Postcode = customer.Postcode,
                PhoneNumber = customer.PhoneNumber,
                Firstname = customer.Firstname,
                Lastname = customer.Lastname,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditCustomer(string id, [Bind(Include = "Firstname,Lastname,Street,City,Postcode,PhoneNumber")] Customer cust)
        {
            //Check if the Model given is Valid
            if (ModelState.IsValid)
            {
                //Get the Customer
                Customer customer = (Customer)await UserManager.FindByIdAsync(id);//Find User by Their Id and cast it as an Customer

                //Update with new Details
                UpdateModel(customer);//Update the new Customer details by using the model

                //Update the context
                IdentityResult result = await UserManager.UpdateAsync(customer);//Update the New Customer Details in the Database

                //If succeeded return to Home
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            //Else return Customer to Edit View
            return View(cust);
        }

    }
}