using System;
using System.Collections.Generic;

namespace WebCats.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime CreatedOn { get; set; }

        public string Role { get; set; }
        //public string Token { get; set; }

        public User(Guid id, string userName, string password, DateTime createdOn, string role)
        {
            Id = id;
            UserName = userName;
            Password = password;
            CreatedOn = createdOn;
            Role = role;
        }
    }
}