using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using RateMy.com.Models;

namespace RateMy.com.Controllers
{
    public class StartPageController : Controller
    {
        public ActionResult Index(StartPageViewModel model)
        {
            // Get all images from DB
            model.ImagesWithInfo = GetImageForDbs();

            return View(model);
        }

        public List<ImageForDb> GetImageForDbs()
        {
            var listOfImageForDb = new List<ImageForDb>();
            var schema = new List<string>();

            var connectionString = ConfigurationManager.ConnectionStrings["Mysql"].ConnectionString;
            using (var con = new MySqlConnection(connectionString))
            {
                const string nameQuery = "SELECT * FROM Images";

                using (var cmd = new MySqlCommand(nameQuery))
                {
                    cmd.Connection = con;
                    con.Open(); // Open connection

                    var reader = cmd.ExecuteReader();
                    while (reader.Read()) // Start reader
                    {

                        var name = reader["Name"].ToString();
                        var email = reader["Email"].ToString();
                        var upVotes = Convert.ToInt32(reader["UpVotes"].ToString());
                        var downVotes = Convert.ToInt32(reader["DownVotes"].ToString());
                        var reports = Convert.ToInt32(reader["Reports"].ToString());
                        var shown = Convert.ToBoolean(reader["Shown"].ToString());
                        var imageName = reader["ImageName"].ToString();
                        var imageType = reader["ImageType"].ToString();
                        var imageFile = reader.GetOrdinal("ImageFile").ToString(); // Problem here
                    }
                    
                    reader.Close(); // Stop reader
                    con.Close(); // Close connection
                }
            }
            return null;
        }

        private Image ByteArrayToImage(Byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            ms.Position = 0;
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
    }
}