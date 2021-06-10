using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LocalTheatreCompany.Models
{
    public class Category
    {
        //Attributes

        //ID
        [Key]
        public int CategoryID { get; set; }

        //Category Name displayed
        [Required]
        [Display(Name = "Category Name")]
        public string Name { get; set; }

        //Ctor
        public Category()
        {
            Posts = new List<Post>();
        }

        //Navigational Properties

        //Posts that belong to this Categories
        public virtual ICollection<Post> Posts { get; set; }
    }
}