using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LocalTheatreCompany.Models
{
    public class LocalTheatreCompanyDbContext : IdentityDbContext<User>
    {
        //Tables
        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public LocalTheatreCompanyDbContext() : base("LocalTheatreCompanyConnection", throwIfV1Schema: false)
        {
            //Intialisation Strategy
            Database.SetInitializer(new DatabaseIntialiser());
        }

        public static LocalTheatreCompanyDbContext Create()
        {
            return new LocalTheatreCompanyDbContext();
        }

    }
}