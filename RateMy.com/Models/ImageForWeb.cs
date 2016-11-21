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
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        public int Reports { get; set; }
        public string ImageName { get; set; }
        public string ImageType { get; set; }
        public Image Image { get; set; }
        public string FullImageFilePathContentFolder { get; set; }
        public string ThumbImageFilePathContentFolder { get; set; }

        public ImageForWeb(int id, string name, string email, int upVotes, int downVotes, int reports, string imageName, string imageType, Image image, string fullImageFilePathContentFolder, string thumbImageFilePathContentFolder)
        {
            Id = id;
            Name = name;
            Email = email;
            UpVotes = upVotes;
            DownVotes = downVotes;
            Reports = reports;
            ImageName = imageName;
            ImageType = imageType;
            Image = image;
            FullImageFilePathContentFolder = fullImageFilePathContentFolder;
            ThumbImageFilePathContentFolder = thumbImageFilePathContentFolder;
        }
    }
}