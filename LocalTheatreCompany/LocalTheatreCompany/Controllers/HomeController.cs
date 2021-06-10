using LocalTheatreCompany.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using LocalTheatreCompany.Models.ViewModels;

namespace LocalTheatreCompany.Controllers
{
    public class HomeController : Controller
    {
        //Instanciate DB Context
        private LocalTheatreCompanyDbContext context = new LocalTheatreCompanyDbContext();

        //Index Controller to Display List of Categories and A list of a few Recent Post's on the Home Page
        public ActionResult Index()
        {
            //Get 3 Recent Posts order by the Recent first
            var posts = context.Posts.Include(p => p.Category).Include(p => p.Staff).OrderByDescending(p => p.DatePosted).Take(3).ToList();

            //Send the List of Categories over to Index Page
            ViewBag.Categories = context.Categories.ToList();

            //Send Posts to the View Named Index
            return View(posts);
        }

        //Contact Page Controller
        public ActionResult Contact()
        {
            return View();
        }
    }
}