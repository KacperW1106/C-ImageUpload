using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ImageUpload.DAL;
using ImageUpload.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageUpload.Controllers
{
    [Route("[controller]/[action]")]
    public class ImageController
    {
        private readonly DB _db;

        // Controller
        public ImageController(DB db)
        {
            _db = db;
        }


        /*********** IMAGE UPLOAD CONTROLLER ***********/
        [HttpPost("{username}")]
        public bool Upload(IFormFile image, string username)
        {
            // We try to read the image and insert the image to the database.
            try
            {
                // We create a memmory stream
                using (var memoryStream = new MemoryStream())
                {
                    image.CopyToAsync(memoryStream);
                    // We create an DB.Image & DB.User with the information posted from the client.
                    DB.Image theImage = new DB.Image();
                    DB.User theUser = _db.Users.Find(username);
                    // Setting all the attributes.
                    theImage.Theimage = memoryStream.ToArray();
                    theImage.User = theUser;
                    // Add the image to the database and write/save to database.
                    _db.Images.Add(theImage);
                    _db.SaveChanges();
                }
                // We return true, as everything worked well.
                return true;
            } catch (Exception e)
            {
                // If we fail to upload/save the image to the database, it gets catched and we return false. We also get the reason of the issue to the console.
                System.Diagnostics.Debug.WriteLine(e.Message);
                return false;
            }
        }



        /*********** GET ALL USERS IMAGES CONTROLLER ***********/
        [HttpGet("{username}")]
        public List<DB.Image> GetAll(string username)
        {
            // We try to load all the images for the given username posted from the client.
            try
            {
                // Creates a list of the class DB.Image and find all the images from the database with the given username
                List<DB.Image> AllImages = _db.Images.Where(u => u.User.Username == username).ToList();
                // Returns all the images.
                return AllImages;
            } catch (Exception e)
            {
                // If we fail/there are noe images for the user, we catch it and return null. We also get the reason of the issue to the console.
                System.Diagnostics.Debug.WriteLine(e.Message);
                return null;
            }
        }



        /*********** DELETE IMAGE WITH ID CONTROLLER ***********/
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            // We try to delete an image, if it fails we catch it at return false.
            try
            {
                // First we try to find the image
                DB.Image DeleteThis = _db.Images.Find(id);
                // Then we remove it
                _db.Images.Remove(DeleteThis);
                // At last we write/save the database and return true.
                _db.SaveChanges();
                return true;
            } catch (Exception e)
            {
                // Return false, as the image either did not exist with the given id or there was a different issues deleting the image from the database.
                // We also get the reason of the issue to the console.
                System.Diagnostics.Debug.WriteLine(e.Message);
                return false;
            }
        }
    }
}
