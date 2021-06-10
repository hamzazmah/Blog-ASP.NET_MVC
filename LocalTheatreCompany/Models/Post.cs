using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LocalTheatreCompany.Models
{
    public class Post
    {
        //Attributes

        //Key
        [Key]
        public int PostID { get; set; }

        //Blog Title
        [Required]
        [Display(Name = "Blog Post Title")]
        public string Title { get; set; }

        //Blog Description with Minimum length required
        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(int.MaxValue, MinimumLength = 200, ErrorMessage = "Description too Short! The Blog Description must be at least 200 characters long")]
        [Display(Name = "Blog Description")]
        public string Description { get; set; }

        //Date the Blog was Posted
        [DataType(DataType.DateTime)]
        [Display(Name = "Date Posted")]
        public DateTime DatePosted { get; set; }

        //Blog Image Url Holder also Uploads the Image to the img folder
        [Required(ErrorMessage = "Please Choose an Image file to Upload!")]
        [Display(Name = "Image")]
        [DataType(DataType.Upload)]
        public string ImageUrl { get; set; }

        //Ctor
        public Post()
        {
            Comments = new List<Comment>();
        }

        //Navigational Properties

        //Comments that are posted on this Post
        public virtual ICollection<Comment> Comments { get; set; }

        //Category this post belongs to
        [Required]
        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        public Category Category { get; set; }

        //Staff who has created this post
        [ForeignKey("Staff")]
        public string UserID { get; set; }
        public Staff Staff { get; set; }
    }
}