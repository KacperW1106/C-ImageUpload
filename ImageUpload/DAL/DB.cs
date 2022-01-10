using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ImageUpload.DAL
{
    public class DB : DbContext
    {
        public DB(DbContextOptions<DB> options) : base(options)
        {
            Database.EnsureCreated();
        }

        // A User class which is used to create a Users table on the database.
        public class User
        {
            [Key]
            public string Username { get; set; }
            public byte[] Password { get; set; }
            public byte[] Salt { get; set; }
        }

        // An Image class which is used to create an images table on the database.
        public class Image
        {
            [Key]
            public int Id { get; set; }
            public byte[] Theimage { get; set; }
            public DB.User User { get; set; }
        }

        // We create a Users table in the database.
        public DbSet<User> Users { get; set; }
        // We create a Images table in the database.
        public DbSet<Image> Images { get; set; }

    }
}
