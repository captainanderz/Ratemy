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
        [Authorize(Roles = "Administrator")]
        public ActionResult Index(AdminViewModel model)
        {
            model.ImagesWithInfo = new CommonHelpers().GetImagesForWebFromDb(true);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult ShowImage(int id)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Mysql"].ConnectionString;
            using (var con = new MySqlConnection(connectionString))
            {
                var query = $"UPDATE Images SET Shown=1 WHERE id = {id}";

                using (var cmd = new MySqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Shown", 1);
                    
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            return RedirectToAction("Index");
        }
    }
}