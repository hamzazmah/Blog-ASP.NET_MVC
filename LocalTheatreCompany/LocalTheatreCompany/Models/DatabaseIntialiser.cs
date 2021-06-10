using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LocalTheatreCompany.Models
{
    public class DatabaseIntialiser : DropCreateDatabaseAlways<LocalTheatreCompanyDbContext>
    {
        protected override void Seed(LocalTheatreCompanyDbContext context)
        {
            base.Seed(context);

            //If there are no Records Stored in the User's Table
            if (!context.Users.Any())
            {
                //Create Some Roles and Store in the Roles Table

                //To Create and Store Roles we Need RoleManager
                RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                //If the Admin Role Doesn't Exist
                if (!roleManager.RoleExists("Admin"))
                {
                    roleManager.Create(new IdentityRole("Admin"));
                }

                if (!roleManager.RoleExists("Staff"))
                {
                    roleManager.Create(new IdentityRole("Staff"));
                }

                if (!roleManager.RoleExists("Customer"))
                {
                    roleManager.Create(new IdentityRole("Customer"));
                }

                //These are Suspended Customers Cannot Post Comments.
                if (!roleManager.RoleExists("Suspended"))
                {
                    roleManager.Create(new IdentityRole("Suspended"));
                }

                // ---------------------------- Creating Users ---------------------

                //To Create Users Customers or Staff we Need UserManager
                UserManager<User> userManager = new UserManager<User>(new UserStore<User>(context));

                //Creating An Admin
                //Checking if the Admin Exists
                if (userManager.FindByName("admin@js.com") == null)
                {
                    //Super Liberal Password Validation for Password for Seeds
                    userManager.PasswordValidator = new PasswordValidator
                    {
                        RequireDigit = false,
                        RequiredLength = 1,
                        RequireLowercase = false,
                        RequireNonLetterOrDigit = false,
                        RequireUppercase = false
                    };

                    var admin = new Staff
                    {
                        UserName = "admin@js.com",
                        Email = "admin@js.com",
                        Firstname = "Adam",
                        Lastname = "Johnson",
                        Street = "12 Greedfy Street",
                        City = "Glasgow",
                        Postcode = "G77 7YT",
                        PhoneNumber = "09876543212",
                        EmailConfirmed = true,
                        IsAdmin = true
                    };

                    //Add Admin to Users Table
                    userManager.Create(admin, "admin123");
                    //Assign it an Admin Role
                    userManager.AddToRoles(admin.Id, "Admin");
                }

                //Adding a Few Staff
                //Checking if Staff is Already Exists
                    var jeff = new Staff
                    {
                        UserName = "jeff@js.com",
                        Email = "jeff@js.com",
                        Firstname = "Jeff",
                        Lastname = "Jefferson",
                        Street = "12 Mint Street",
                        City = "Minty",
                        Postcode = "G77 7YT",
                        PhoneNumber = "09876543212",
                        EmailConfirmed = true,
                        IsAdmin = false
                    };

                    //Add Jeff Staff to Users Table
                    userManager.Create(jeff, "staff1");
                    //Assign it an Staff Role
                    userManager.AddToRoles(jeff.Id, "Staff");


                    var xander = new Staff
                    {
                        UserName = "xander@js.com",
                        Email = "xander@js.com",
                        Firstname = "Xander",
                        Lastname = "Alexie",
                        Street = "1 Scot Street",
                        City = "Air",
                        Postcode = "G77 7YT",
                        PhoneNumber= "09876543232",
                        EmailConfirmed = true,
                        IsAdmin = false
                    };

                    //Add Xander Staff to Users Table
                    userManager.Create(xander, "staff2");
                    //Assign it an Staff Role
                    userManager.AddToRoles(xander.Id, "Staff");


                //Adding a Few customers
                if (userManager.FindByName("billy@gmail.com") == null)
                {
                    var billy = new Customer
                    {
                        UserName = "billy@gmail.com",
                        Email = "billy@gmail.com",
                        Firstname = "Billy",
                        Lastname = "Bill",
                        Street = "22 Timmy Street",
                        City = "Edinburgh",
                        Postcode = "EH1 2BT",
                        PhoneNumber = "09876543243",
                        EmailConfirmed = true,
                        IsSuspended = false
                    };

                    //Add Billy Customer to Users Table
                    userManager.Create(billy, "customer1");
                    //Assign it an Customer Role
                    userManager.AddToRoles(billy.Id, "Customer");
                }

                if (userManager.FindByName("timmy@gmail.com") == null)
                {
                    var timmy = new Customer
                    {
                        UserName = "timmy@gmail.com",
                        Email = "timmy@gmail.com",
                        Firstname = "Timmy",
                        Lastname = "Turner",
                        Street = "22 Smith Street",
                        City = "Edinburgh",
                        Postcode = "EE2 3BT",
                        PhoneNumber = "09876543245",
                        EmailConfirmed = true,
                        IsSuspended = false
                    };

                    //Add Timmy Customer to Users Table
                    userManager.Create(timmy, "customer2");
                    //Assign it an Customer Role
                    userManager.AddToRoles(timmy.Id, "Customer");
                }

                if (userManager.FindByName("smith@gmail.com") == null)
                {
                    var smith = new Customer
                    {
                        UserName = "smith@gmail.com",
                        Email = "smith@gmail.com",
                        Firstname = "Smith",
                        Lastname = "Golds",
                        Street = "22 Street",
                        City = "Glasgow",
                        Postcode = "G22 3BT",
                        PhoneNumber = "09876543224",
                        IsSuspended = false
                    };

                    //Add Smith Customer to Users Table
                    userManager.Create(smith, "customer3");
                    //Assign it an Customer Role
                    userManager.AddToRoles(smith.Id, "Customer");
                }

                if (userManager.FindByName("shack@gmail.com") == null)
                {
                    var shack = new Customer
                    {
                        UserName = "shack@gmail.com",
                        Email = "shack@gmail.com",
                        Firstname = "Shack",
                        Lastname = "Shake",
                        Street = "22 Queens",
                        City = "London",
                        Postcode = "L89 9TY",
                        PhoneNumber = "09876543542",
                        IsSuspended = false
                    };

                    //Add Shack Customer to Users Table
                    userManager.Create(shack, "customer4");
                    //Assign it an Customer Role
                    userManager.AddToRoles(shack.Id, "Customer");
                }

                if (userManager.FindByName("alex@gmail.com") == null)
                {
                    var alex = new Customer
                    {
                        UserName = "alex@gmail.com",
                        Email = "alex@gmail.com",
                        Firstname = "alex",
                        Lastname = "Shake",
                        Street = "22 Queens",
                        City = "London",
                        Postcode = "L89 9TY",
                        PhoneNumber = "09876543209",
                        IsSuspended = true
                    };

                    //Add Jeff Customer to Users Table
                    userManager.Create(alex, "suspended1");
                    //Assign it an Customer Role
                    userManager.AddToRoles(alex.Id, "Suspended");
                }

                // ---------------------------- Creating Users ---------------------


                // Adding Categories

                 Category cat1 = new Category
                {
                    Name = "Announcements"
                };
                Category cat2 = new Category
                {
                    Name = "News"
                };
                Category cat3 = new Category
                {
                    Name = "Reviews"
                };

                //Adding Categories to the DB
                context.Categories.Add(cat1);
                context.Categories.Add(cat2);
                context.Categories.Add(cat3);


                //Adding Posts

                Post post1 = new Post 
                {
                    Title = "1st Post",
                    Description = "This is 1st Post. Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum. ",
                    DatePosted = DateTime.Now,
                    ImageUrl = "/img/post-bg.jpg",
                    Category = cat1,//Category it Belongs to
                    CategoryID = cat1.CategoryID,
                    Staff = xander,//User who created the Post
                    UserID = xander.Id
                };

                Post post2 = new Post
                {
                    Title = "2nd Post",
                    Description = "This is a 2nd Post. Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum. ",
                    DatePosted = DateTime.Now.AddDays(-14),
                    ImageUrl = "/img/post-bg.jpg",
                    Category = cat2,
                    CategoryID = cat2.CategoryID,
                    Staff = xander,
                    UserID = xander.Id
                  
                };

               
                Post post3 = new Post
                {
                    Title = "3rd Post",
                    Description = "This is a 3rd Post. Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum. ",
                    DatePosted = DateTime.Now.AddDays(-14),
                    ImageUrl = "/img/post1.jpg",
                    Category = cat2,
                    CategoryID = cat2.CategoryID,
                    Staff = xander,
                    UserID = xander.Id
                  
                };

                Post post4 = new Post
                {
                    Title = "4th Post",
                    Description = "This is a 4th Post. Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum. ",
                    DatePosted = DateTime.Now.AddDays(-14),
                    ImageUrl = "/img/post1.jpg",
                    Category = cat2,
                    CategoryID = cat2.CategoryID,
                    Staff = jeff,
                    UserID = jeff.Id

                };

                Post post5 = new Post
                {
                    Title = "5th Post",
                    Description = "This is a 5th Post. Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum. ",
                    DatePosted = DateTime.Now.AddDays(-14),
                    ImageUrl = "/img/post1.jpg",
                    Category = cat3,
                    CategoryID = cat3.CategoryID,
                    Staff = jeff,
                    UserID = jeff.Id

                };


                //Adding Post's to the Database
                context.Posts.Add(post2);
                context.Posts.Add(post1);
                context.Posts.Add(post3);
                context.Posts.Add(post4);
                context.Posts.Add(post5);

                //Adding Comments

                var cmnt1 = new Comment
                {
                    CommentDescription = "Nice Going",
                    DatePosted = DateTime.Now,
                    IsAccepted = true,
                    User = xander,
                    UserID = xander.Id,
                    Post = post1,
                    PostID= post1.PostID
                };

                var cmnt2 = new Comment
                {
                    CommentDescription = "Well Going",
                    DatePosted = DateTime.Now.AddDays(-25),
                    IsAccepted = true,
                    User = xander,
                    UserID = xander.Id,
                    Post = post3,
                    PostID = post3.PostID
                };

                var cmnt3 = new Comment
                {
                    CommentDescription = "Nice Work",
                    DatePosted = DateTime.Now.AddDays(-5),
                    IsAccepted = false,
                    User = xander,
                    UserID = xander.Id,
                    Post = post2,
                    PostID = post2.PostID
                };

                //Adding Comments to the Database
                context.Comments.Add(cmnt1);
                context.Comments.Add(cmnt2);
                context.Comments.Add(cmnt3);

                //Saving these Changes
                context.SaveChanges();
            }
        }
    }
}