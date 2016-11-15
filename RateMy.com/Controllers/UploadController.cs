using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
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
                    // Create imageForDb file
                    var imageForDb = new CommonHelpers().CreateImageForDb(name, email, uploadFile);

                    // Upload to DB process
                    using (var imageStream = imageForDb.ImageStream)
                    {
                        using (var br = new BinaryReader(imageStream))
                        {
                            var bytes = br.ReadBytes((int)imageStream.Length);
                            var connectionString = ConfigurationManager.ConnectionStrings["Mysql"].ConnectionString;
                            using (var con = new MySqlConnection(connectionString))
                            {
                                const string query =
                                    "INSERT INTO Images(Name, Email, UpVotes, DownVotes, Reports, Shown, ImageName, ImageType, ImageFile) VALUES (@Name, @Email, @UpVotes, @DownVotes, @Reports, @Shown, @ImageName, @ImageType, @ImageFile)";

                                using (var cmd = new MySqlCommand(query))
                                {
                                    cmd.Connection = con;
                                    cmd.Parameters.AddWithValue("@Name", imageForDb.Name);
                                    cmd.Parameters.AddWithValue("@Email", imageForDb.Email);
                                    cmd.Parameters.AddWithValue("@UpVotes", imageForDb.UpVotes);
                                    cmd.Parameters.AddWithValue("@DownVotes", imageForDb.DownVotes);
                                    cmd.Parameters.AddWithValue("@Reports", imageForDb.Reports);
                                    cmd.Parameters.AddWithValue("@Shown", imageForDb.Shown);
                                    cmd.Parameters.AddWithValue("@ImageName", imageForDb.ImageName);
                                    cmd.Parameters.AddWithValue("@ImageType", imageForDb.ImageType);
                                    cmd.Parameters.AddWithValue("@ImageFile", bytes);
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                            }
                        }
                    }
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