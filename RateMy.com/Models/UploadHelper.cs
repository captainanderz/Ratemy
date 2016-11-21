using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace RateMy.com.Models
{
    public class UploadHelper
    {
        public void ShowImage(int id)
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
        }

        /// <summary>
        /// Uploads image with properties to mysql Db
        /// </summary>
        /// <param name="name">Name of the uploader</param>
        /// <param name="email">Email of the uploader</param>
        /// <param name="uploadFile">The uploaders</param>
        public void UploadImageToDb(string name, string email, HttpPostedFileBase uploadFile)
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
}