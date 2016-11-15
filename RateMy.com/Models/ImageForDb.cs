using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace RateMy.com.Models
{
    public class ImageForDb
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        public int Reports { get; set; }
        public bool Shown { get; set; }
        public string ImageName { get; set; }
        public string ImageType { get; set; }
        public Stream ImageStream { get; set; }

        public ImageForDb(string name, string email, int upVotes, int downVotes, int reports, bool shown, string imageName,string imageType,Stream imageStream)
        {
            Name = name;
            Email = email;
            UpVotes = upVotes;
            DownVotes = downVotes;
            Reports = reports;
            Shown = shown;
            ImageName = imageName;
            ImageType = imageType;
            ImageStream = imageStream;
        }
    }
}