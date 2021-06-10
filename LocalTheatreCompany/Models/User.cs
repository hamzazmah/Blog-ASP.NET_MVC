using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace LocalTheatreCompany.Models
{
    public abstract class User : IdentityUser
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
        [Display(Name = "Postal Code")]
        public string Postcode { get; set; }

        [Required]
        [Display(Name = "Mobile Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\d{11})$", ErrorMessage = "Please Enter a 11 Digit Phone Number!")]
        public override string PhoneNumber { get; set; }

        //Ctor
        public User()
        {
            Comments = new List<Comment>();
        }

        //Navigational Property

        //Comments that the Users have Posted
        public virtual ICollection<Comment> Comments { get; set; }


        //Instanciate UserManager to get User's Current Role
        private ApplicationUserManager userManager;

        //To Get the Current Role of the User who is Logged In
        [NotMapped]
        public string CurrentRole
        {
            get 
            {
                if (userManager == null)
                {
                    userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                }

                return userManager.GetRoles(Id).Single();
            }
        }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}