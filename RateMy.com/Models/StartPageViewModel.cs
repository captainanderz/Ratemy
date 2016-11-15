using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace RateMy.com.Models
{
    public class StartPageViewModel
    {
        public List<ImageForDb> ImagesWithInfo { get; set; }

        public Image FullImage(Stream imageStream)
        {
            return Image.FromStream(imageStream);
        }
        
    }
}