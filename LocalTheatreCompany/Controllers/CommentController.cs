using LocalTheatreCompany.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LocalTheatreCompany.Controllers
{
    //This is For Adding, Editing and Removing Comments 
    [Authorize(Roles = "Admin,Staff,Customer")]//Not Available to Suspended or Not logged in Users
    [HandleError]
    public class CommentController : Controller
    {
        //Instanciate The Db Context
        private LocalTheatreCompanyDbContext context = new LocalTheatreCompanyDbContext();

        //Allows Users to Add new Comments
        [HttpGet]
        public ActionResult AddComment(int? PostID)
        {
            //Check if the ID is not null
            if (PostID == null)
            {
                return HttpNotFound();
            }

            //This is so User can go Back to the Post by Clicking back button
            ViewBag.PostID = PostID;

            //Return View
            return View();
        }

        
        [HttpPost]
        public ActionResult AddComment([Bind(Include = "CommentID, CommentDescription")] Comment cmnt, int? PostID)
        {
            //Check if the Id is Not Null
            if (PostID == null)
            {
                return HttpNotFound();
            }

            //Check if the Model returned is Valid
            if (ModelState.IsValid)
            {
                //Get the current User Who posted this Comment
                User user = context.Users.Find(User.Identity.GetUserId());

                //If a Customer Post's a Comment the Comment is Sent for Approval so IsAccepted is False and Cannot be seen by others until accepted by the Admin
                if ( user != null && user.CurrentRole.Equals("Customer"))
                {
                    cmnt.IsAccepted = false;
                }
                else//If the Current User is an Admin or Staff then it is accepted and can be seen by others
                {
                    cmnt.IsAccepted = true;
                }
                
                //Add the Date the Comment was Posted
                cmnt.DatePosted = DateTime.Now;
                //The User who Posted this Comment
                cmnt.UserID = User.Identity.GetUserId();
                //The Post this Comment Belongs to
                cmnt.PostID = (int)PostID;

                //Add the Comment to the Database
                context.Comments.Add(cmnt);
                //Save Changer
                context.SaveChanges();

                //If Sucessfull Redirect to the Post page 
                return RedirectToAction("DetailPost", "Post",  new { id = PostID } );
            }

            //Else stay on that page
            ViewBag.PostID = PostID;
            return View(cmnt);
        }

        //To Edit the Comment
        [HttpGet]
        public ActionResult EditComment(int? CommentID, int? PostID)
        {
            //Check if the Given Id's are Valid
            if (CommentID == null || PostID == null )
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            //Find a Comment by the ID
            Comment comment = context.Comments.Find(CommentID);

            //Get the Current User
            var currentUser = context.Users.Find(User.Identity.GetUserId());

            //Send the User with this model
            comment.User = currentUser;

            //If Comment doesn't Exist then Return a Not Found error
            if (comment == null)
            {
                return HttpNotFound();
            }

            //Return the Post ID so User can go Back to the Post
            ViewBag.PostID = (int)PostID;
            //Return the Current User for Admin Purposes
            ViewBag.CurrentUser = currentUser;

            //Return comment to the View
            return View(comment);
        }

        // POST: Member/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditComment(Comment comment, int? PostID, string UserID)
        {
            //Check if the ID is not Null
            if (PostID == null)
            {
                return HttpNotFound();
            }

            // If the Comment passed to the Edit is not Null
            if (ModelState.IsValid)
            {
                //Record the New Date Comment was Edited
                comment.DatePosted = DateTime.Now;
                
                //Assign the PostID
                comment.PostID = (int)PostID;

                //Gets the Id of the User Who Commented it Assign it as the Foreign key in the Comment
                comment.UserID = UserID;

                //Updates the database
                context.Entry(comment).State = EntityState.Modified;
                //Save changes to the Database
                context.SaveChanges();

                //Redirect to The Post
                return RedirectToAction("DetailPost", "Post", new { id = PostID });
            }

            //Return the Comment to the Edit form
            ViewBag.PostID = (int)PostID;
            return View(comment);
        }


        //User's Can delete their Comments 
        [HttpGet]
        public ActionResult DeleteComment(int? CommentID, int? PostID)
        {
            //Check if the ID is Valid
            if (CommentID == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            //Find a Comment by the ID
            Comment comment = context.Comments.Find(CommentID);

            //If Comment does'nt Exist then Return a Not Found error
            if (comment == null)
            {
                return HttpNotFound();
            }

            //Return the Post Id
            ViewBag.PostID = (int)PostID;

            //Return the Comment to the Edit form
            return View(comment);
        }

        [HttpPost, ActionName("DeleteComment")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? CommentID, int? PostID)
        {
            //Check if the ID is Valid
            if (CommentID == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            //Find the Comment by ID
            Comment comment = context.Comments.Find(CommentID);

            //If Comment does'nt Exist then Return a Not Found error
            if (comment == null)
            {
                return HttpNotFound();
            }

            //remove Comment from Posts Table
            context.Comments.Remove(comment);

            //Save Changers
            context.SaveChanges();

            //Redirect to the Post
            return RedirectToAction("DetailPost", "Post", new { id = PostID });
        }
    }
}