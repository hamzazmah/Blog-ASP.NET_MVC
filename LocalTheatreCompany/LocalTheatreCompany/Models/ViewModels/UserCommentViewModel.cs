using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocalTheatreCompany.Models.ViewModels
{
    //ViewModel to Get all the Users Username who have Commented
    public class UserCommentViewModel
    {
        //Attributes 

        public string UserID { get; set; }

        public string Username { get; set; }
    }
}