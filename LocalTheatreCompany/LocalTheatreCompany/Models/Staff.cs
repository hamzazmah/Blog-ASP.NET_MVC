using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocalTheatreCompany.Models
{
    public class Staff : User
    {
        //Staff Specific Posts
        public bool IsAdmin { get; set; }

        //Ctor
        public Staff()
        {
            Posts = new List<Post>();
        }

        //Navigational Property

        //Posts that were Created by the Staff USer
        public virtual ICollection<Post> Posts { get; set; }

    }
}