using System;
using System.Collections.Generic;
using System.Linq;
using ImageUpload.DAL;
using ImageUpload.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageUpload.Controllers
{
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly DB _db;

        // Session attribute
        private const string _loggedIn = "loggedIn";

        // Controller
        public UserController(DB db)
        {
            _db = db;
        }



        /*********** REGISTER USER CONTROLLER ***********/
        [HttpPost]
        public bool Register([FromBody] User user)
        {
            // We check if there is an exisiting user with the given username.
            DB.User findUser = _db.Users.Find(user.Username);
            // If a user is found, we can not create an account!
            if (findUser != null)
            {
                return false;
            }
            // The user was not found, so we try to create an account and set a session for the account.
            try
            {
                // If all goes well, we create a DB.User
                DB.User regUser = new DB.User();
                // We assign the attributes to the regUser
                regUser.Username = user.Username;
                byte[] salt = Password.CreateSalt();
                regUser.Password = Password.CreateHash(user.Password, salt);
                regUser.Salt = salt;
                // We add the user to the database
                _db.Users.Add(regUser);
                // We save the changes to the database
                _db.SaveChanges();
                HttpContext.Session.SetString(_loggedIn, "loggedIn");
                return true;
            } catch (Exception e)
            {
                // If for some reason an account could not be created, we tell the user and log the issue to the console.
                System.Diagnostics.Debug.WriteLine(e.Message);
                return false;
            }
        }



        /*********** LOGIN USER CONTROLLER ***********/
        [HttpPost]
        public bool Login([FromBody] User user)
        {
            // We try to find the user with the username from the user poisted from the client.
            DB.User findUser = _db.Users.Find(user.Username);
            if (findUser != null)
            {
                // We found the user, now lets check if the user has inserted the correct password!

                // We create a hash with the inserted password and the found users salt.@
                byte[] hash = Password.CreateHash(user.Password, findUser.Salt);
                if (hash.SequenceEqual(findUser.Password))
                {
                    // The hash sequense was equal, meaning the user inserted the correct password. We log the person in!
                    HttpContext.Session.SetString(_loggedIn, "loggedIn");
                    return true;
                }
                // Hash sequence was wrong, we dont log the person in!
                return false;
            }
            // Did not find the user
            return false;
        }



        /*********** CHECK IF USER IS LOGGED IN CONTROLLER ***********/
        [HttpGet]
        public bool CheckIfLoggedIn()
        {
            // If the session string _loggedIn is "loggedIn", the user is logged in!
            if (HttpContext.Session.GetString(_loggedIn) == "loggedIn")
            {
                return true;
            } else
            {
                // If the string is not "loggedIn", the user is not logged in.
                return false;
            }
        }


        /*********** LOGOUT USER CONTROLLER ***********/
        [HttpDelete]
        public void Logout()
        {
            // To logout, we simply set the "_loggedIn" sessions string as empty.
            HttpContext.Session.SetString(_loggedIn, "");
        }

    }
}
