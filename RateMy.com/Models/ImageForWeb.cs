using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace RateMy.com.Models
{
    public class ImageForWeb
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        public int Reports { get; set; }
        public bool Shown { get; set; }
        public string ImageName { get; set; }
        public string ImageType { get; set; }
        public Image Image { get; set; }
        public string FilePathContentFolder { get; set; }

        public ImageForWeb(string name, string email, int upVotes, int downVotes, int reports, bool shown, string imageName, string imageType, Image image, string filePathContentFolder)
        {
            Name = name;
            Email = email;
            UpVotes = upVotes;
            DownVotes = downVotes;
            Reports = reports;
            Shown = shown;
            ImageName = imageName;
            ImageType = imageType;
            Image = image;
            FilePathContentFolder = filePathContentFolder;
        }
    }
}