using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ProjeTakip
{
    public static class Crypto
    {
        public static string Hash(string value)
        {

            byte[] encodedPassword = new UTF8Encoding().GetBytes(value);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);
            string encoded = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();

            return encoded;
            
        }



    }
}