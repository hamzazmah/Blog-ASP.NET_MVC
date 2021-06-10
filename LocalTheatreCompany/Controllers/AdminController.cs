using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using LocalTheatreCompany.Models;
using LocalTheatreCompany.Models.ViewModels;

namespace LocalTheatreCompany.Controllers
{
    //Admin Controls for Users, Comments and Posts
    [Authorize(Roles = "Admin")]//Only Admin can Access this Controller
    public class AdminController : AccountController
    {
        //Initiating the Database Context
        private Models.LocalTheatreCompanyDbContext db = new LocalTheatreCompanyDbContext();

        public AdminController() : base()
        {

        }

        public AdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager) : base(userManager, signInManager)
        {

        }

        //Index View to Show All Available Admin Controls
        public ActionResult Index()
        {
            return View();
        }

        //Users Admin Controls

        //View all Users
        public ActionResult Users()
        {
            //Get the Current User Id so They cannot edit or Delete themselves
            string userId = User.Identity.GetUserId();

            //Get All Users
            var users = db.Users.Where(u => u.Id != userId).ToList();

            //Get all Users who are Customers
            var customers = db.Users.Where(p => p is Customer).ToList();

            //Return these to the Viwe
            ViewBag.Customers = customers;

            //Sending the List of Users to the Index View
            return View(users);
        }

        //Create a New Staff Member
        [HttpGet]
        public ActionResult CreateStaff()
        {
            //Using the CreateStaff ViewModel
            CreateStaffViewModel staff = new CreateStaffViewModel();

            //Get All the roles from the Database and store them as a SelectedListItem so roles can be displayed as a DropDownList
            var roles = db.Roles.Where(r => r.Name != "Admin" && r.Name != "Suspended").Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            }).ToList();

            //Assign this list of roles to Staff Roles Property
            staff.Roles = roles;

            //Send the Staff Model to the View
            return View(staff);
        }

        [HttpPost]
        public ActionResult CreateStaff(CreateStaffViewModel model)
        {
            //If Model is Not Null
            if (ModelState.IsValid)
            {
                //build the Employee
                Staff newStaff = new Staff
                {
                    UserName = model.Email,
                    Email = model.Email,
                    EmailConfirmed = true,
                    Street = model.Street,
                    City = model.City,
                    Postcode = model.PostCode,
                    PhoneNumber = model.PhoneNumber,
                    PhoneNumberConfirmed = true,
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    //A New Staff is not Admin
                    IsAdmin = false
                };

                //Create the User and Store in the Database and Pass the password to be Hashed
                var result = UserManager.Create(newStaff, model.Password);
                //if the User is Stored in the Database Sucessfully
                if (result.Succeeded)
                {
                    //Then add user to the Selected Role
                    UserManager.AddToRole(newStaff.Id, model.Role);

                    //Redirect to the Users View
                    return RedirectToAction("Users", "Admin");
                }
            }
            //Something went Wrong so Go back to create Staff View
            return View(model);
        }

        // Admin Can Edit Staff and Customers

        //Edit Staff
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult EditStaff(string id)
        {
            //Check if the Id is Valid
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Find the Staff by there ID
            Staff staff = db.Users.Find(id) as Staff;

            //Check if the staff is not Null
            if (staff == null)
            {
                return HttpNotFound();
            }

            //Send Staff Details to the View
            return View(new EditStaffViewModel
            {
                Street = staff.Street,
                City = staff.City,
                PostCode = staff.Postcode,
                PhoneNumber = staff.PhoneNumber,
                FirstName = staff.Firstname,
                LastName = staff.Lastname,
                IsAdmin = staff.IsAdmin
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditStaff(string id, [Bind(Include = "FirstName,LastName,Street,City,PostCode,PhoneNumber,IsAdmin")] EditStaffViewModel model)
        {
            //Check if the Model is Valid
            if (ModelState.IsValid)
            {
                Staff staff = (Staff)await UserManager.FindByIdAsync(id);//Find User by Their Id and cast it as an Staff

                UpdateModel(staff);//Update the new Staff details by using the model

                //For Changing the Role

                //Get Old Role ID
                var oldRoleId = staff.Roles.SingleOrDefault().RoleId;
                //Get Old Role Name
                var oldRoleName = db.Roles.SingleOrDefault(r => r.Id == oldRoleId).Name;

                //Check if Admin promoted the Staff memebr and they are not admin role
                if (staff.IsAdmin == true && oldRoleName != "Admin")
                {
                    //Change their Role From Staff to Admin
                    UserManager.RemoveFromRole(staff.Id, "Staff");
                    UserManager.AddToRole(staff.Id, "Admin");
                }
                //Check if Admin Demoted tge Staff member and They are not already a Staff Role
                else if (staff.IsAdmin == false && oldRoleName != "Staff")
                {
                    //Change their Role From Admin to Staff
                    UserManager.RemoveFromRole(staff.Id, "Admin");
                    UserManager.AddToRole(staff.Id, "Staff");
                }

                //Update the Details in the db
                IdentityResult result = await UserManager.UpdateAsync(staff);//Update the New Staff Details in the Database

                //If successfull return to the Users view
                if (result.Succeeded)
                {
                    return RedirectToAction("Users", "Admin");
                }
            }

            //Else Return to the Edit View
            return View(model);
        }

        //Edit Customer
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

            //Check if Customer is not Null
            if (customer == null)
            {
                return HttpNotFound();
            }

            //Send Customer Details to the View
            return View(new EditCustomerViewModel
            {
                Street = customer.Street,
                City = customer.City,
                PostCode = customer.Postcode,
                PhoneNumber = customer.PhoneNumber,
                FirstName = customer.Firstname,
                LastName = customer.Lastname,
                IsSuspended = customer.IsSuspended
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditCustomer(string id, [Bind(Include = "FirstName,LastName,Street,City,PostCode,PhoneNumber,IsSuspended")] EditCustomerViewModel model)
        {
            //Check if the Model is Valid
            if (ModelState.IsValid)
            {
                Customer customer = (Customer)await UserManager.FindByIdAsync(id);//Find User by Their Id and cast it as an Customer

                UpdateModel(customer);//Update the new Customer details by using the model

                //For Suspending or Unsuspending Customers from Commenting

                //Get the Old Role ID
                var oldRoleId = customer.Roles.SingleOrDefault().RoleId;
                //Get the Old Role Name
                var oldRoleName = db.Roles.SingleOrDefault(r => r.Id == oldRoleId).Name;

                //Check if Admin Suspended the Customer and the Customer is not Already Suspended Role
                if (customer.IsSuspended == true && oldRoleName != "Suspended")
                {
                    //Chaneg Role from Customer to Suspended
                    UserManager.RemoveFromRole(customer.Id, "Customer");
                    UserManager.AddToRole(customer.Id, "Suspended");
                }
                //Check if Admin Un-Suspended the Customer and the Customer is not Already Customer Role
                else if (customer.IsSuspended == false && oldRoleName != "Customer")
                {
                    //Chaneg Role from Suspended to Customer
                    UserManager.RemoveFromRole(customer.Id, "Suspended");
                    UserManager.AddToRole(customer.Id, "Customer");
                }

                //Update the Customer in the Db
                IdentityResult result = await UserManager.UpdateAsync(customer);//Update the New Customer Details in the Database

                //If Suceeded in Updating return to Users View
                if (result.Succeeded)
                {
                    return RedirectToAction("Users", "Admin");
                }
            }

            //Else return the model to the Edit View
            return View(model);
        }

        //Admin can View User Details
        public ActionResult Details(string id)
        {
            //Check if the User id is not Null
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Get the User from db
            User user = db.Users.Find(id);
            
            //Check if the User is Valid
            if (user == null)
            {
                return HttpNotFound();
            }

            //If User is Staff return these details to DetailsStaff View
            if (user is Staff)
            {
                return View("DetailsStaff", (Staff)user);
            }

            //If User is Customer Return these User Details to DetailsCustomer viwe
            if (user is Customer)
            {
                return View("DetailsCustomer", (Customer)user);
            }

            //Else Error
            return HttpNotFound();
        }

        //Admin can Delete Users
        [HttpGet]
        public async Task<ActionResult> DeleteUser(string id)
        {
            //Check if the User Id is Valid
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            //Find user by ID
            User user = await UserManager.FindByIdAsync(id);

            //If User does'nt Exist then Return a Not Found error
            if (user == null)
            {
                return HttpNotFound();
            }

            //Return User to the View
            return View(user);
        }

        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteUserConfirmed(string id)
        {
            //Check if the User Id is Valid
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            //Get the User 
            User user = await UserManager.FindByIdAsync(id);

            //If User does'nt Exist then Return a Not Found error
            if (user == null)
            {
                return HttpNotFound();
            }

            //Check if the User is Staff then Remove all the posts
            if (user is Staff)
            {
                //Find the Posts of this User by the Foreign Key UserID
                var posts = db.Posts.Where(p => p.UserID == user.Id);

                //Remove Each Post
                foreach (var item in posts)
                {
                    db.Posts.Remove(item);
                }
            }

            //For all users remove their corresponding Comments
            //Get all the Comments Posted by this User
            var comments = db.Comments.Where(c => c.UserID == user.Id);
            //Remove Each Comment
            foreach (var item in comments)
            {
                db.Comments.Remove(item);
            }

            //Save Changers
            db.SaveChanges();

            //Delete user
            await UserManager.DeleteAsync(user);

            //Return to the Users View
            return RedirectToAction("Users", "Admin");
        }

        //Displays All the Comments that Require Moderation
        public ActionResult UserComments()
        {
            //Get all the Comments that Needs to be Accepted
            var comments = db.Comments.Where(c => c.IsAccepted == false);

            //For each Comment
            foreach (var comment in comments)
            {
                //Find the Employee by there ID
                var user = db.Users.Find(comment.UserID);
                
                comment.User = user;
            }

            //Return Them to the view
            return View(comments);
        }

        //Categories

        public ActionResult ViewAllCategories()
        {
            //Return all the Categories
            return View(db.Categories.ToList());
        }

        //Add a New category
        [HttpGet]
        public ActionResult CreateCategory()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCategory([Bind(Include = "CategoryID, Name")] Category category)
        {
            //Check if the Model is Valid
            if (ModelState.IsValid)
            {
                //Add the New Category and Return to ViewAllCategories
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("ViewAllCategories");
            }

            return View(category);
        }

        //Edit Category
        [HttpGet]
        public ActionResult EditCategory(int? id)
        {
            //Check if the Id is Valid
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            //Get the Category from db
            Category category = db.Categories.Find(id);

            //Check if Category is not Null
            if (category == null)
            {
                return HttpNotFound();
            }

            //Return to View
            return View(category);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategory([Bind(Include = "CategoryID, Name")] Category category)
        {
            //Check if the model is Valid
            if (ModelState.IsValid)
            {
                //Update the Category
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewAllCategories");
            }

            return View(category);
        }

        //Delete Category
        public ActionResult DeleteCategory(int? id)
        {
            //Check if the Id is valid
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            //Get the Category from Db
            Category category = db.Categories.Find(id);

            //Check if its Valid
            if (category == null)
            {
                return HttpNotFound();
            }

            //return To view
            return View(category);
        }

        //Delete Category
        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCategory(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();

            return RedirectToAction("ViewAllCategories");
        }


        //Dispose of the Db Context
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}