using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace RateMy.com.Models
{
    public class CommonHelpers
    {
        public ImageForDb CreateImageForDb(string name, string email, HttpPostedFileBase uploadFile)
        {
            var uploadFileAsStream = uploadFile.InputStream;
            name = FirstLetterUppercase(name); // First letter in name to Uppercase
            var imageForDb = new ImageForDb(name, email, 0, 0, 0, false,uploadFile.FileName, uploadFile.ContentType, uploadFileAsStream);
            return imageForDb;
        }

        public string FirstLetterUppercase(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            var a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
    }
}