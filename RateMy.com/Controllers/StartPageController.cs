using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;
using RateMy.com.Models;

namespace RateMy.com.Controllers
{
    public class StartPageController : Controller
    {
        public ActionResult Index(StartPageViewModel model)
        {
            // Get all images from DB
            model.ImagesWithInfo = new CommonHelpers().GetImagesForWebFromDb();

            return View(model);
        }
    }
}