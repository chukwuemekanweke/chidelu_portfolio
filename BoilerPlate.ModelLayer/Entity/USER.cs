using System;
using System.Collections.Generic;
using System.Text;

namespace BoilerPlate.ModelLayer.Entity
{
    public class USER
    {
        public Guid Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string OtherName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ImageURL { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string Username { get; set; }
        public string IdentityUserId { get; set; }
    }
}
