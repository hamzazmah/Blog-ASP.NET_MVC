using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LocalTheatreCompany.Models
{
    public class Comment
    {
        //Attributes

        //ID
        [Key]
        public int CommentID { get; set; }

        //Comment Details
        [Required]
        [Display(Name = "Comment Description")]
        public string CommentDescription { get; set; }

        //Date the Comment is Posted
        [Display(Name = "Date Posted")]
        public DateTime DatePosted { get; set; }

        //For Admin Validation
        public bool IsAccepted { get; set; }

        //Navigational Properties
        //User it Belongs to
        [ForeignKey("User")]
        public string UserID { get; set; }
        public User User { get; set; }

        //Post it Belongs to
        [ForeignKey("Post")]
        public int PostID { get; set; }
        public Post Post { get; set; }

    }
}