using System;
using System.Collections.Generic;

#nullable disable

namespace MvcWebApp.Models
{
    public partial class UsersDetail
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
