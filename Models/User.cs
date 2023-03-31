using System;
using System.Collections.Generic;

#nullable disable

namespace MvcWebApp.Models
{
    public partial class User
    {
        public User()
        {
            UsersDetails = new HashSet<UsersDetail>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public virtual ICollection<UsersDetail> UsersDetails { get; set; }
    }
}
