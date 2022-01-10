using System;
using System.ComponentModel.DataAnnotations;

namespace ImageUpload.Models
{
    // This User class is created to send a User class with a string username and password from the client to the server.
    public class User
    {
        [Key]
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
