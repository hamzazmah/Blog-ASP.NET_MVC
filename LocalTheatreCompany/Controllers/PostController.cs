using LocalTheatreCompany.Models;
using LocalTheatreCompany.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace LocalTheatreCompany.Controllers
{
    //Post Controller Handels Displaing a List of Categorised Post's or All Posts and Displaying the details of a Slected Post
    public class PostController : Controller
    {
        //Instanciate DB Context
        private LocalTheatreCompanyDbContext context = new LocalTheatreCompanyDbContext();

        //This Displays a List of Categorise and List of All the Posts
        public ActionResult Index()
        {
            //Get all the Posts by all the User's by Newest First as a List
            var posts = context.Posts.Include(p => p.Category).Include(p => p.Staff).OrderByDescending(p => p.DatePosted).ToList();

            //Loop through
            foreach (var item in posts)
            {
                context.Entry(item).Reload();//Refresh Entities
            }

            //Send the List of Categories over to Index Page
            ViewBag.Categories = context.Categories.ToList();

            //Send the Currently Signed User's ID so If the User is Staff and It Matches the Post User id then Show Options such as Edit or Delete
            ViewBag.UserID = User.Identity.GetUserId();

            //Send Posts Collection to the View Named Posts
            return View("Posts", posts);
        }

        //This Gets the Category ID when specific Category Button is Pressed and Displays all the Posts for that Category
        public ActionResult Posts(int? id)
        {
            //Getting all the Posts that are in a specific Category
            var posts = context.Posts.Include(p => p.Category).Include(p => p.Staff).Where(p => p.CategoryID == id).OrderByDescending(p => p.DatePosted).ToList();

            //Send All Categories in a ViewBag
            ViewBag.Categories = context.Categories.ToList();

            //Send Posts Collection to the View Named Posts
            return View("Posts", posts);
        }

        //When a Post is Cliked on Form home to Blog Posts or staff Posts page This is Responsible for Displaying all the Blog Details and Blog Specific Comments and Comments options
        public ActionResult DetailPost(int? id)
        {
            //Check if the Id is Valid
            if (id == null)
            {
                return HttpNotFound();
            }

            //Find the Post by the Given ID
            var post = context.Posts.Find(id);

            //Get a List of all the USers
            List<User> users = context.Users.ToList();

            //Get the UserId of Currently Signed In user
            var userId = User.Identity.GetUserId();

            //Get All the User Id and Usernames so they can be displayed
            List<UserCommentViewModel> userCmnts = new List<UserCommentViewModel>();
            foreach (var item in users)
            {
                var cmnt = new UserCommentViewModel 
                {
                    UserID = item.Id,
                    Username = item.UserName
                };
                userCmnts.Add(cmnt);
            }

            //This is to see if the User Customer is Suspended from commenting
            ViewBag.Customer = context.Users.Find(userId) as Customer;
            //This is to Display Comments Username
            ViewBag.Users = userCmnts;
            //This is to Check if the Comments of the Post belong to the the Currently Signed in User who can then Edit or Delete Them 
            ViewBag.UserID = userId;

            //Return the Model to the View
            return View(post);
        }
    }
}