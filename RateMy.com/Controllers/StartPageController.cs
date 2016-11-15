using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
            var schema = new List<string>();
            var connectionString = ConfigurationManager.ConnectionStrings["Mysql"].ConnectionString;
            using (var con = new MySqlConnection(connectionString))
            {
                const string nameQuery = "SELECT Name FROM Images";

                using (var cmd = new MySqlCommand(nameQuery))
                {
                    cmd.Connection = con;
                    con.Open(); // Open connection

                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    while (reader.HasRows)
                    {
                        for (var row = 0; row <= reader.FieldCount; row++)
                        {
                            schema.Add(reader.GetString(row)/*reader.GetString(reader.GetOrdinal("Name"))*/);
                        }
                        
                    }
                    

                    con.Close(); // Close connection
                }
            }
            return null;
        }
    }
}