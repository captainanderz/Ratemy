using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using RateMy.com.Models;

namespace RateMy.com.Controllers
{
    public class AdminController : Controller
    {
        //[Authorize(Roles = "Administrator")]
        public ActionResult Index(AdminViewModel model)
        {
            model.ImagesWithInfo = new CommonHelpers().GetImagesForWebFromDb(true);
            return View(model);
        }

        [HttpPost]
        //[Authorize(Roles = "Administrator")]
        public ActionResult ShowImage(int id)
        {
            new UploadHelper().ShowImage(id);

            return RedirectToAction("Index");
        }
    }
}