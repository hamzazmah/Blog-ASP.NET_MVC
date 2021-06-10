using LocalTheatreCompany.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.IO;

namespace LocalTheatreCompany.Controllers
{
    [Authorize(Roles = "Staff,Admin")]//Only Registered Staff and Admin and Member Role can Access this
    public class StaffController : Controller
    {
        //Initialise Database Context
        private LocalTheatreCompanyDbContext context = new LocalTheatreCompanyDbContext();

        //This Index action is called when Registered Staff Member Clicks the Link to My Post's
        //This Method Returns a List of Post's that were Created by the Logged in Staff
        
        public ActionResult Index()
        {
            //Select all the Posts from the Post Table
            //Including Foreign Key Categories and user
            var posts = context.Posts.Include(p => p.Category).Include(p => p.Staff);

            //Get the ID of the Logged In User
            //user Id is a String
            var userID = User.Identity.GetUserId();

            Staff staff = context.Users.Find(userID) as Staff;

            //If Staff does'nt Exist then Return a Not Found error
            if (staff == null)
            {
                return HttpNotFound();
            }

            //Specific options for an Admin Role if the User is Admin then all Post's are Returned
            if (staff.CurrentRole.Equals("Admin"))
            {
                ViewBag.Title = "List of all the User Post's";
            }
            //Options for a Staff Role
            else if (staff.CurrentRole.Equals("Staff"))
            {
                //From the List of Posts
                //Select only the Ones that have the same UserID to the Logged in User
                posts = posts.Where(p => p.UserID == userID);

                ViewBag.Title = "List of all Your Post's";
            }
            //Send the Lists of Posts to the Index View
            return View(posts.OrderByDescending(p => p.DatePosted).ToList());
        }

        //Staff can Add a new Post
        public ActionResult AddPost()
        {
            //send the List of Categories to the View
            ViewBag.CategoryId = new SelectList(context.Categories, "CategoryID", "Name");

            //Return to the Viwe
            return View();
        }


        [HttpPost]
        public ActionResult AddPost([Bind(Include = "PostID, Title, Description, ImageUrl, CategoryID")] Post post)
        {
            //Get the First file uploaded
            HttpPostedFileBase file = Request.Files[0];
            // If the Post passed to the Edit is not Null
            if (ModelState.IsValid)
            {
                //Record the New Date for the Post
                post.DatePosted = DateTime.Now;

                //Gets the Id of the User Logged In and Assigns it as the Foreign key in the Post
                post.UserID = User.Identity.GetUserId();

                //For File Upload
                if (file != null)
                {
                    //Get the file Name
                    var fileName = Path.GetFileName(file.FileName);
                    //Map the Path with /img folder
                    var path = Path.Combine(Server.MapPath("/img"), fileName);
                    //Save the file to that Path
                    file.SaveAs(path);
                    //Save ImageUrl in the Post
                    post.ImageUrl = "/img/" + fileName;
                }

                //Add the Post to the Posts Table
                context.Posts.Add(post);

                //Save changes to the Database
                context.SaveChanges();

                //Redirect to Index
                return RedirectToAction("Index");
            }
            //Otherwise if the Post is null Get a List of all the Categories from the categories Table
            ViewBag.CategoryId = new SelectList(context.Categories, "CategoryID", "Name", post.CategoryID);

            //Return the Post to the Edit form
            return View(post);
        }

        //Edit Post
        [HttpGet]
        public ActionResult EditPost(int? id)
        {
            //Check if ID is Valid
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            //Find a Post by the ID
            Post post = context.Posts.Find(id);

            //If post does'nt Exist then Return a Not Found error
            if (post == null)
            {
                return HttpNotFound();
            }

            //Get a List of all the Categories from the categories Table
            ViewBag.CategoryId = new SelectList(context.Categories, "CategoryID", "Name", post.CategoryID);

            //Return the Post to the View
            return View(post);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost([Bind(Include = "PostID, Title, Description, ImageUrl, CategoryID")] Post post, string UserID)
        {
            //Get the First File Uploaded
            HttpPostedFileBase file = Request.Files[0];

            // If the Post passed to the Edit is not Null
            if (ModelState.IsValid && file != null && UserID != null)
            {
                //Record the New and Expiry Date Post was Edited
                post.DatePosted = DateTime.Now;

                post.UserID = UserID;
                // For File Upload
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("/img"), fileName);
                string newImg = "/img/" + fileName;

                //Save the File
                file.SaveAs(path);
                //Save the Image Url to the Post
                post.ImageUrl = newImg;


                //Updates the database
                context.Entry(post).State = EntityState.Modified;
                //Save changes to the Database
                context.SaveChanges();

                //Redirect to Index
                return RedirectToAction("Index");
            }
            //Otherwise if the Post is null Get a List of all the Categories from the categories Table
            ViewBag.CategoryId = new SelectList(context.Categories, "CategoryID", "Name", post.CategoryID);

            //Return the Post to the Edit form
            return View(post);
        }

        //Staff can Delete theri own Post and Admin can Delete all the Posts
        [HttpGet]
        public ActionResult DeletePost(int? id)
        {
            //Check if the Id is Valid
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            //Find a Post by the ID
            Post post = context.Posts.Find(id);

            //Find the Category which is a Foriegn Key for this post
            var category = context.Categories.Find(post.CategoryID);

            //Get the Staff who Created this Post
            Staff staff = (Staff)context.Users.Find(post.UserID);

            // Staff and the category 
            post.Category = category;
            post.Staff = staff;

            //If post does'nt Exist then Return a Not Found error
            if (post == null)
            {
                return HttpNotFound();
            }

            //Retuen to the View
            return View(post);
        }


        [HttpPost, ActionName("DeletePost")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePostConfirmed(int? id)
        {
            //Check if the Id is Valid
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            //Find the Post by ID
            Post post = context.Posts.Find(id);

            //If post does'nt Exist then Return a Not Found error
            if (post == null)
            {
                return HttpNotFound();
            }

            //Get all the Comments the Post has
            var comments = context.Comments.Where(p => p.PostID == id);
            //Delete Each one of the,
            foreach (var item in comments)
            {
                context.Comments.Remove(item);
            }

            //remove post from Posts Table
            context.Posts.Remove(post);

            //Save Changers
            context.SaveChanges();

            //Redirect to Index
            return RedirectToAction("Index");
        }
    } 
}