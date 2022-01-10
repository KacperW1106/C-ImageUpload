using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ImageUpload.Models
{
    public class Password
    {

        // Method for creating a hash with the use of a strin password and a salt
        public static byte[] CreateHash(string password, byte[] salt) 
        {
            // We create a KeyDerivation
            byte[] kd = KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA512,
                    iterationCount: 1000,
                    numBytesRequested: 32
                );
            return kd;
        }

        // Method to create a salt.
        public static byte[] CreateSalt()
        {
            var csp = new RNGCryptoServiceProvider();
            var salt = new byte[24];
            csp.GetBytes(new byte[24]);
            return salt;
        }
    }
}
