using System.Linq;
using System.Web;
using System.Web.Mvc;
using RateMy.com.Models;

namespace RateMy.com.Controllers
{
    public class UploadController : Controller
    {
        public LayoutViewModel LayoutViewModel = new LayoutViewModel();

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase uploadFile, string name, string email)
        {
            if (ModelState.IsValid && (uploadFile != null))
            {
                //Allowed types
                string[] validExtensions = {"image/jpg", "image/jpeg", "image/bmp", "image/gif", "image/png" };

                //Checking file size
                if ((uploadFile.ContentLength == 0) && (uploadFile.ContentLength > 10000000))
                {
                    LayoutViewModel.ErrorMessage = "Could not upload: File too big (max size 10mb)";
                    ViewData["StatusMsg"] = @"Could not upload: File too big (max size 10mb)";
                }
                //Check extension
                else if (validExtensions.Contains(uploadFile.ContentType) == false)
                {
                    LayoutViewModel.ErrorMessage = "Could not upload: Illigal file type!";
                    ViewData["StatusMsg"] = "Could not upload: Illigal file type!";
                }
                else
                {
                    // Upload image with properties to Db
                    new UploadHelper().UploadImageToDb(name, email, uploadFile);
                }
            }
            else
            {
                LayoutViewModel.ErrorMessage = "Image is NULL";
            }


            return RedirectToAction("Index", "StartPage", LayoutViewModel);
        }
    }
}